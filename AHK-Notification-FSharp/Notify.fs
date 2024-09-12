namespace AHK_Notification_FSharp
open System
open System.IO
module internal Utils =
    let str (o: Object) = o |> function null -> "" | x -> x.ToString()
    let isNotEmpty = str >> fun x -> x.Trim().Length > 0

module Notify =
    open Utils
    type Parameters =
        {
            TitleSize: int
            TitleColor: string
            TitleFont: string
            MessageSize: int
            MessageColor: string
            LogPath: string
            MessageFont: string
            NotificationTitle: string
            NotificationText: string
            BackgroundColor: string
            PadSize: int
            IgnoreHover: bool
            MaximumMessageLength: int
            LogCallsPath: string
        }
        static member Default title message =
            {
                TitleSize = 30
                TitleColor = "7FA2CF"
                LogPath = ""
                TitleFont = "Segoe UI Light"
                MessageSize = 11
                MessageColor = "White"
                MessageFont = "Segoe UI"
                BackgroundColor = "2A2B2F"
                PadSize = 30
                IgnoreHover = false
                NotificationTitle = title
                NotificationText = message
                MaximumMessageLength = 100
                LogCallsPath = ""
            }

    let private logCallParameters logCallsPath exePath args =
        if logCallsPath |> str |> Seq.length > 0 then 
            let exe = System.IO.Path.GetFullPath(exePath)
            let logMessage = $@"{exe} {args}"
            System.IO.File.AppendAllText(logCallsPath, logMessage + Environment.NewLine) |> ignore
            
    let getExeDir =
        let assembly = System.Reflection.Assembly.GetExecutingAssembly()
        let location = assembly.Location
        System.IO.Path.GetDirectoryName(location)

    let Notify (parameters: Parameters) =
        let exePath = System.IO.Path.Combine(getExeDir, @"AHK-Notification\AHK-Notification.exe")
        if System.IO.File.Exists(exePath) then
            let message = parameters.NotificationText.Substring(0, Math.Min(parameters.NotificationText.Length, parameters.MaximumMessageLength))
            let args =
                [
                   //Op: Auto
                   "notificationText" , message
                   "notificationTitle", parameters.NotificationTitle
                   "logPath"          , parameters.LogPath
                   "backgroundColor"  , parameters.BackgroundColor
                   "padSize"          , parameters.PadSize |> str
                   "titleSize"        , parameters.TitleSize |> str
                   "titleColor"       , parameters.TitleColor
                   "titleFont"        , parameters.TitleFont
                   "messageSize"      , parameters.MessageSize |> str
                   "messageColor"     , parameters.MessageColor
                   "messageFont"      , parameters.MessageFont
                   "ignoreHover"      , if parameters.IgnoreHover then "1" else "0"
                   //Op: End
                ]
                |> Seq.filter (snd >> isNotEmpty)
                |> Seq.map (fun (k, v) -> 
                    $@"""%s{k}=%s{v}""")
                |> String.concat " "
            logCallParameters parameters.LogCallsPath exePath args
            System.Diagnostics.Process.Start(exePath, args) |> ignore
        else
            failwithf "%s doesn't exist." exePath
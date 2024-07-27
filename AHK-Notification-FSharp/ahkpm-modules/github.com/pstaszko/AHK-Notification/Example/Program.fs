let notify msg =
    let p =
        let p = AHK_Notification_FSharp.Notify.Parameters.Default "" msg
        { p with
            LogPath = @"c:\temp\notifications.txt"
            PadSize = 0
            LogCallsPath = @"c:\temp\notifs.log.txt"
            MaximumMessageLength = 500
            IgnoreHover = true
        }
    AHK_Notification_FSharp.Notify.Notify p
//os._SysAlert <- Progs.AutoHotkey.Growl
notify "hi"
()
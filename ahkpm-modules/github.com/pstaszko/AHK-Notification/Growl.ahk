Growl(message,title="",MessageType="Standard Message"){
	if RegExMatch(message,"^[0| ]+$")
		return
	notifScript=%a_scriptdir%\ahkpm-modules\github.com\pstaszko\AHK-Notification\AHK-Notification.ahk
	notifScript=%a_scriptdir%\ahkpm-modules\github.com\pstaszko\AHK-Notification\AHK-Notification.exe
	IfExist %notifScript%
	{
		if (message or title )
		{
			title:=strreplace(title,"\","\\")
			message:=strreplace(message,"\","\\")
			message:=SubStr(message, 1, 100)
			cmd=%A_AhkPath% "%notifScript%" "notificationText=%message%" "notificationTitle=%title%" "logFile=c:\temp\notification.txt" "backgroundColor=4e5057" "padsize=0" "ignoreHover=1"
			cmd="%notifScript%" "notificationText=%message%" "notificationTitle=%title%" "logFile=c:\temp\notification.txt" "backgroundColor=4e5057" "padsize=0" "ignoreHover=1"
			run %cmd%,,hide
		}
	}else{
		msgbox %notifScript% doesn't exist
	}
}
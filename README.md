# ShellLauncher #

## Introduction ##
C# Application for launching two different process in two shell configurable. The first process is a TCP server application exploited by the second process.

## Executable Utilization ##

ShellLauncher can replace the Windows Standard Shell (explorer.exe or cmd.exe), set the key Shell under HKEY_Current_User\Software\Microsoft\Windows NT\CurrentVersion\Winlogon\Shell (See [MSDN Help](MSDN Help "https://msdn.microsoft.com/en-us/library/ms838576(v=winembedded.5).aspx") for details).

ShellLauncher.exe accepts different kind of command line arguments. In the following the type of launching.

### Request Help ###

*%PATHSHELL%>ShellLauncher.exe ?*

Provides the type of use.

### Inline Parameters  ###

*%PATHSHELL%>ShellLauncher.exe -firstTask=<NAME_FIRST_TASK-secondTask=<NAME_SECOND_TASK-connectionAddress=<ADDRESS_CONNECTION-TCPPort=<TCPPORT_CONNECTION[-intervalTasks=<INTERVAL_TASKS>] [-leaveShellAlwaysActive=<LEAVESHELLACTIVEAFTERTASKEXIT>]*

The list of parameters (Mandatory and optionals) are listed in the following: 

- **firstTask**: The global path of the first process (executable or batch file) to launch. It is intended to be a server exploited by the second process;
- **secondTask**: The global path of the second process that communicates with the first one by means of TCP Connection;
- **connectionAddress**: The connection address of the first process (N.B. ShellLauncher tries to perform TCP connection to a specified IP address and TCP port before launching the second process);
- **TCPPort**: The TCP Port of the first process;
- **intervalTasks** *[Optional]*: The interval (expressed in milliseconds) between the TCP connection attempt and the launch of the second process;
- **leaveShellAlwaysActive** *[Optional]*: Flag that indicates whether leaves both shells active (set to 1) or not (set to 0).


### Specified Configuration files  ###

ShellLauncher -settingsLauncher=<PathFileSettings>
It extract settings (see the format of first choice set by line)

ShellLauncher
It tries to extract settings from file pathsettingsLauncher.conf

THIRD CHOICE:


@echo off
Nuget.exe restore "Source\NLog.Xml.netfx35.sln"
Nuget.exe restore "Source\NLog.Xml.netfx40.sln"
Nuget.exe restore "Source\NLog.Xml.netfx45.sln"

NuGet.exe install MSBuildTasks -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
NuGet.exe install xunit.runner.console -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive

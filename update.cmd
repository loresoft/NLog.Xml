@echo off
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml\NLog.Xml.netfx40.csproj" -r "Source\packages"
git checkout  "Source\NLog.Xml\packages.config"
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml\NLog.Xml.netfx45.csproj" -r "Source\packages"

git checkout  "Source\NLog.Xml\packages.config"
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml\NLog.Fluent.netfx40.csproj" -r "Source\packages"
git checkout  "Source\NLog.Xml\packages.config"
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml\NLog.Fluent.netfx45.csproj" -r "Source\packages"

"Source\.nuget\NuGet.exe" update "Source\NLog.Xml.ConsoleTest\NLog.Xml.ConsoleTest.netfx40.csproj" -r "Source\packages"
git checkout  "Source\NLog.Xml.ConsoleTest\packages.config"
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml.ConsoleTest\NLog.Xml.ConsoleTest.netfx45.csproj" -r "Source\packages"

"Source\.nuget\NuGet.exe" update "Source\NLog.Xml.Test\NLog.Xml.Test.netfx40.csproj" -r "Source\packages"
git checkout  "Source\NLog.Xml.Test\packages.config"
"Source\.nuget\NuGet.exe" update "Source\NLog.Xml.Test\NLog.Xml.Test.netfx45.csproj" -r "Source\packages"

msbuild master.proj -t:Refresh
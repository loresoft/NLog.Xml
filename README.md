#NLog.Xml

NLog XML layout and fluent logging.

[![Build status](https://ci.appveyor.com/api/projects/status/9m108vtfq3t3lyc7)](https://ci.appveyor.com/project/LoreSoft/nlog-xml)
[![Version](https://img.shields.io/nuget/v/NLog.Xml.svg)](https://www.nuget.org/packages/nlog.xml)

##Download

The NLog.Xml library is available on nuget.org via package name `NLog.Xml`.

To install NLog.Xml, run the following command in the Package Manager Console

    PM> Install-Package NLog.Xml
    
More information about NuGet package avaliable at
<https://nuget.org/packages/NLog.Xml>

##Development Builds


Development builds are available on the myget.org feed.  A development build is promoted to the main NuGet feed when it's determined to be stable. 

In your Package Manager settings add the following package source for development builds:
<http://www.myget.org/F/loresoft/>

##Xml Layout

Use the XML layout renderer

    <target ... layout="${xml}" />


Add custom properties to the xml document

```xml
<target ...>
    <layout xsi:type="XmlLayout">
        <property name="ThreadID" layout="${threadid}" />
        <property name="ThreadName" layout="${threadname}" />
        <property name="ProcessID" layout="${processid}" />
        <property name="ProcessName" layout="${processname:fullName=true}" />
        <property name="UserName" layout="${windows-identity}" />
    </layout>
</target>
```

##Fluent

Writing info message via fluent API.

    _logger.Info()
        .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
        .Property("Test", "InfoWrite")
        .Write();

Writing error message.

    try
    {
        string text = File.ReadAllText(path);
    }
    catch (Exception ex)
    {
        _logger.Error()
            .Message("Error reading file '{0}'.", path)
            .Exception(ex)
            .Property("Test", "ErrorWrite")
            .Write();
    }

##Caller Info

Use the static Log class so you don't have to include loggers in all of classes.  The static Log class using .net 4.5 caller info to get the logger from the file name. The needed attributes are included to support targeting .net 4.0 runtime.

Writing info message via static Log class with fluent API.

    Log.Info()
        .Message("This is a test fluent message.")
        .Property("Test", "InfoWrite")
        .Write();

Writing error message.

    try
    {
        string text = File.ReadAllText(path);
    }
    catch (Exception ex)
    {
        Log.Error()
            .Message("Error reading file '{0}'.", path)
            .Exception(ex)
            .Property("Test", "ErrorWrite")
            .Write();
    }

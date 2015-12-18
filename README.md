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
    <property name="MachineName" layout="${machinename}"/>
    <property name="ThreadID" layout="${threadid}" />
    <property name="ThreadName" layout="${threadname}" />
    <property name="ProcessID" layout="${processid}" />
    <property name="ProcessName" layout="${processname:fullName=true}" />
    <property name="UserName" layout="${windows-identity}" />
  </layout>
</target>
```

Write XML to file using header and footer to set xml root node.

```xml
<target xsi:type="File"
        name="fileXmlName"
        header="&lt;nlog&gt;"
        footer="&lt;/nlog&gt;"
        fileName="${logDirectory}/log.xml"
        archiveFileName="${logDirectory}/archives/log.{#}.xml"
        archiveEvery="Day"
        archiveNumbering="Rolling"
        maxArchiveFiles="7"
        concurrentWrites="true"
        createDirs="true"
        autoFlush="true">
  <layout xsi:type="XmlLayout">
    <property name="MachineName" layout="${machinename}"/>
    <property name="ThreadID" layout="${threadid}" />
    <property name="ThreadName" layout="${threadname}" />
    <property name="ProcessID" layout="${processid}" />
    <property name="ProcessName" layout="${processname:fullName=true}" />
    <property name="UserName" layout="${windows-identity}" />
  </layout>
</target>
```

Send MSMQ message as XML

```xml
<target xsi:type="MSMQ"
        name="messageQueue"
        recoverable="true"
        createQueueIfNotExists="true"
        label="${logger}"
        queue=".\private$\logging" >
  <layout xsi:type="XmlLayout">
    <property name="MachineName" layout="${machinename}"/>
    <property name="ThreadID" layout="${threadid}" />
    <property name="ThreadName" layout="${threadname}" />
    <property name="ProcessID" layout="${processid}" />
    <property name="ProcessName" layout="${processname:fullName=true}" />
    <property name="UserName" layout="${windows-identity}" />
  </layout>
</target>
```
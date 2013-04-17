NLog.Xml
===========

NLog XML layout and fluent logging.

Xml Layout
===========

Use the XML layout renderer

    <target ... layout="${xml}" />


Add custom properties to the xml document

    <target ...>
      <layout xsi:type="XmlLayout">
        <property name="ThreadID" layout="${threadid}" />
        <property name="ThreadName" layout="${threadname}" />
        <property name="ProcessID" layout="${processid}" />
        <property name="ProcessName" layout="${processname:fullName=true}" />
        <property name="UserName" layout="${windows-identity}" />
      </layout>
    </target>


Fluent
===========

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

Caller Info
===========

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

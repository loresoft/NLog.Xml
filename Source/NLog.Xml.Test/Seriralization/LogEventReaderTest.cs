using System;
using System.IO;
using System.Xml;
using FluentAssertions;
using NLog.Serialization;
using Xunit;

namespace NLog.Xml.Test.Seriralization
{
    public class LogEventReaderTest
    {
        [Fact]
        public void Reader()
        {
            string xml = @"<LogEvent>
  <SequenceID>1</SequenceID>
  <TimeStamp>2012-09-24T09:09:59.5433403-05:00</TimeStamp>
  <Level>Error</Level>
  <LoggerName>LogEventWriterTest</LoggerName>
  <Message>Error reading file 'blah.txt'.</Message>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
           
            log.Should().NotBeNull();
            log.SequenceID.Should().Be(1);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T09:09:59.5433403-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().Be("LogEventWriterTest");
            log.Message.Should().Be("Error reading file 'blah.txt'.");
        }

        [Fact]
        public void ReaderNoWhiteSpace()
        {
            string xml = @"<LogEvent><SequenceID>1</SequenceID><TimeStamp>2012-09-24T09:09:59.5433403-05:00</TimeStamp><Level>Error</Level><LoggerName>LogEventWriterTest</LoggerName><Message>Error reading file 'blah.txt'.</Message></LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();
            log.SequenceID.Should().Be(1);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T09:09:59.5433403-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().Be("LogEventWriterTest");
            log.Message.Should().Be("Error reading file 'blah.txt'.");
        }

        [Fact]
        public void ReaderExtraElement()
        {
            string xml = @"<LogEvent>
  <SequenceID>1</SequenceID>
  <TimeStamp>2012-09-24T09:09:59.5433403-05:00</TimeStamp>
  <Blah>Extra Text</Blah>
  <Level>Error</Level>
  <LoggerName>LogEventWriterTest</LoggerName>
  <Message>Error reading file 'blah.txt'.</Message>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();
            log.SequenceID.Should().Be(1);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T09:09:59.5433403-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().Be("LogEventWriterTest");
            log.Message.Should().Be("Error reading file 'blah.txt'.");
        }

        [Fact]
        public void ReaderFull()
        {
            string xml = @"
<LogEvent>
  <TimeStamp>2012-09-24T12:07:12.604366-05:00</TimeStamp>
  <Level>Error</Level>
  <LoggerName>LogEventWriterTest</LoggerName>
  <Message>Error reading file 'blah.txt'.</Message>
  <Error>
    <TypeName>System.ApplicationException</TypeName>
    <MethodName>NestedErrorWrite</MethodName>
    <ModuleName>NLog.Xml.Test</ModuleName>
    <ModuleVersion>1.0.0.1</ModuleVersion>
    <Message>Error reading file.</Message>
    <Source>NLog.Xml.Test</Source>
    <StackTrace>   at NLog.Xml.Test.Seriralization.LogEventWriterTest.NestedErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 70</StackTrace>
    <ExceptionText>System.ApplicationException: Error reading file. ---&gt; System.IO.FileNotFoundException: Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.NestedErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 66
   --- End of inner exception stack trace ---
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.NestedErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 70</ExceptionText>
    <InnerError>
      <TypeName>System.IO.FileNotFoundException</TypeName>
      <MethodName>WinIOError</MethodName>
      <ModuleName>mscorlib</ModuleName>
      <ModuleVersion>4.0.0.0</ModuleVersion>
      <Message>Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.</Message>
      <Source>mscorlib</Source>
      <StackTrace>   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.NestedErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 66</StackTrace>
      <ExceptionText>System.IO.FileNotFoundException: Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.
File name: 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.NestedErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 66</ExceptionText>
    </InnerError>
  </Error>
  <Properties>
    <Property>
      <Name>MachineName</Name>
      <Value>MSPW-WELTEP</Value>
    </Property>
    <Property>
      <Name>CallerMemberName</Name>
      <Value>NestedErrorWrite</Value>
    </Property>
    <Property>
      <Name>CallerFilePath</Name>
      <Value>c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs</Value>
    </Property>
    <Property>
      <Name>CallerLineNumber</Name>
      <Value>75</Value>
    </Property>
    <Property>
      <Name>Test</Name>
      <Value>ErrorWrite</Value>
    </Property>
  </Properties>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();
            log.SequenceID.Should().Be(0);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T12:07:12.604366-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().Be("LogEventWriterTest");
            log.Message.Should().Be("Error reading file 'blah.txt'.");

            log.Error.Should().NotBeNull();
            log.Error.TypeName.Should().Be("System.ApplicationException");
            log.Error.MethodName.Should().Be("NestedErrorWrite");
            log.Error.ModuleName.Should().Be("NLog.Xml.Test");
            log.Error.ModuleVersion.Should().Be("1.0.0.1");
            log.Error.Message.Should().Be("Error reading file.");
            log.Error.Source.Should().Be("NLog.Xml.Test");

            log.Error.InnerError.Should().NotBeNull();
            log.Error.InnerError.TypeName.Should().Be("System.IO.FileNotFoundException");
            log.Error.InnerError.MethodName.Should().Be("WinIOError");
            log.Error.InnerError.ModuleName.Should().Be("mscorlib");
            log.Error.InnerError.ModuleVersion.Should().Be("4.0.0.0");
            log.Error.InnerError.Message.Should().Be(@"Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.");
            log.Error.InnerError.Source.Should().Be("mscorlib");

            log.Properties.Should().NotBeNull();
            log.Properties.Should().NotBeEmpty();
            log.Properties.Count.Should().Be(5);

            log.Properties[0].Name.Should().Be("MachineName");
            log.Properties[0].Value.Should().Be("MSPW-WELTEP");

            log.Properties[1].Name.Should().Be("CallerMemberName");
            log.Properties[1].Value.Should().Be("NestedErrorWrite");

            log.Properties[2].Name.Should().Be("CallerFilePath");
            log.Properties[2].Value.Should().Be(@"c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs");

            log.Properties[3].Name.Should().Be("CallerLineNumber");
            log.Properties[3].Value.Should().Be("75");

            log.Properties[4].Name.Should().Be("Test");
            log.Properties[4].Value.Should().Be("ErrorWrite");

        }

        [Fact]
        public void ReaderFullExtra()
        {
            string xml = @"
<LogEvent>
  <TimeStamp>2012-09-24T13:36:06.210787-05:00</TimeStamp>
  <Level>Error</Level>
  <Blah>Extra Text</Blah>
  <LoggerName>LogEventWriterTest</LoggerName>
  <Message>Error reading file 'blah.txt'.</Message>
  <Error>
    <TypeName>System.IO.FileNotFoundException</TypeName>
    <MethodName>WinIOError</MethodName>
    <Blah>Extra Text</Blah>
    <ModuleName>mscorlib</ModuleName>
    <ModuleVersion>4.0.0.0</ModuleVersion>
    <Message>Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.</Message>
    <Source>mscorlib</Source>
    <StackTrace>   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.ErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 23</StackTrace>
    <ExceptionText>System.IO.FileNotFoundException: Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.
File name: 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at NLog.Xml.Test.Seriralization.LogEventWriterTest.ErrorWrite() in c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs:line 23</ExceptionText>
  </Error>
  <Properties>
    <Blah>Extra Text</Blah>
    <Property>
      <Name>MachineName</Name>
      <Blah>Extra Text</Blah>
      <Value>MSPW-WELTEP</Value>
    </Property>
    <Property>
      <Name>CallerMemberName</Name>
      <Value>ErrorWrite</Value>
    </Property>
    <Property>
      <Name>CallerFilePath</Name>
      <Value>c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs</Value>
    </Property>
    <Property>
      <Name>CallerLineNumber</Name>
      <Value>27</Value>
    </Property>
    <Property>
      <Name>Test</Name>
      <Value>ErrorWrite</Value>
    </Property>
  </Properties>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();
            log.SequenceID.Should().Be(0);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T13:36:06.210787-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().Be("LogEventWriterTest");
            log.Message.Should().Be("Error reading file 'blah.txt'.");

            log.Error.Should().NotBeNull();
            log.Error.TypeName.Should().Be("System.IO.FileNotFoundException");
            log.Error.MethodName.Should().Be("WinIOError");
            log.Error.ModuleName.Should().Be("mscorlib");
            log.Error.ModuleVersion.Should().Be("4.0.0.0");
            log.Error.Message.Should().Be(@"Could not find file 'C:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\bin\Debug\blah.txt'.");
            log.Error.Source.Should().Be("mscorlib");

            log.Properties.Should().NotBeNull();
            log.Properties.Should().NotBeEmpty();
            log.Properties.Count.Should().Be(5);

            log.Properties[0].Name.Should().Be("MachineName");
            log.Properties[0].Value.Should().Be("MSPW-WELTEP");

            log.Properties[1].Name.Should().Be("CallerMemberName");
            log.Properties[1].Value.Should().Be("ErrorWrite");

            log.Properties[2].Name.Should().Be("CallerFilePath");
            log.Properties[2].Value.Should().Be(@"c:\Projects\github\NLog.Xml\Source\NLog.Xml.Test\Seriralization\LogEventWriterTest.cs");

            log.Properties[3].Name.Should().Be("CallerLineNumber");
            log.Properties[3].Value.Should().Be("27");

            log.Properties[4].Name.Should().Be("Test");
            log.Properties[4].Value.Should().Be("ErrorWrite");

        }

        [Fact]
        public void ReaderNullLogger()
        {
            string xml = @"<LogEvent>
  <SequenceID>1</SequenceID>
  <TimeStamp>2012-09-24T09:09:59.5433403-05:00</TimeStamp>
  <Level>Error</Level>
  <Message>Error reading file 'blah.txt'.</Message>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();
            log.SequenceID.Should().Be(1);
            log.TimeStamp.Should().Be(DateTime.Parse("2012-09-24T09:09:59.5433403-05:00"));
            log.Level.Should().Be("Error");
            log.LoggerName.Should().BeNull();
            log.Message.Should().Be("Error reading file 'blah.txt'.");
        }
        
        [Fact]
        public void ReadNull()
        {
            string xml = "<LogEvent />";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
            log.Should().NotBeNull();
        }

        [Fact]
        public void ReadChildNull()
        {
            string xml = "<LogEvent><Level /></LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
            log.Should().NotBeNull();
        }
        
        [Fact]
        public void ReadPropertyNull()
        {
            string xml = "<LogEvent><Properties /></LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
            log.Should().NotBeNull();
        }
        
        [Fact]
        public void ReadErrorNull()
        {
            string xml = "<LogEvent><Error /></LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
            log.Should().NotBeNull();
        }

        [Fact]
        public void ReadPropertiesNoWhiteSpace()
        {
            string xml = @"<LogEvent><Properties><Property><Name>Test</Name><Value>ErrorWrite</Value></Property></Properties></LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);
            
            log.Should().NotBeNull();

            log.Properties.Should().NotBeNull();
            log.Properties.Should().NotBeEmpty();
            log.Properties.Count.Should().Be(1);

            log.Properties[0].Name.Should().Be("Test");
            log.Properties[0].Value.Should().Be("ErrorWrite");

        }

        [Fact]
        public void ReadPropertiesWhiteSpace()
        {
            string xml = @"<LogEvent>
    <Properties>
        <Property>
            <Name>Test</Name>
            <Value>ErrorWrite</Value>
        </Property>
    </Properties>
</LogEvent>";

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr);

            var logReader = new LogEventReader();
            var log = logReader.Read(reader);

            log.Should().NotBeNull();

            log.Properties.Should().NotBeNull();
            log.Properties.Should().NotBeEmpty();
            log.Properties.Count.Should().Be(1);

            log.Properties[0].Name.Should().Be("Test");
            log.Properties[0].Value.Should().Be("ErrorWrite");

        }
    }




}



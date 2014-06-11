using System;
using System.IO;
using System.Xml;
using FluentAssertions;
using NLog.Fluent;
using NLog.Model;
using NLog.Serialization;
using Xunit;

namespace NLog.Xml.Test.Seriralization
{
    public class LogEventWriterTest
    {
        [Fact]
        public void ErrorWrite()
        {
            LogEvent logEvent = new LogEvent();

            string path = "blah.txt";
            try
            {
                string text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                var log = Log.Error()
                    .Message("Error reading file '{0}'.", path)
                    .Exception(ex)
                    .Property("Test", "ErrorWrite")
                    .LogEventInfo;

                logEvent.Populate(log);
            }

            string fileName = string.Format("LogEvent-{0}.xml", DateTime.Now.Ticks);
            
            string xml = logEvent.Save();
            File.WriteAllText(fileName, xml);

            string outputFileName = Path.ChangeExtension(fileName, ".Writer.xml");
            
            var settings = new XmlWriterSettings { Indent = true };
            var writer = XmlWriter.Create(outputFileName, settings);
            var eventWriter = new LogEventWriter();

            eventWriter.Write(writer, logEvent);
            writer.Flush();
            writer.Close();

            string newXml = File.ReadAllText(outputFileName);
            var newEvent = LogEvent.Load(newXml);
            newEvent.Should().NotBeNull();
        }

        [Fact]
        public void NestedErrorWrite()
        {
            LogEvent logEvent = new LogEvent();

            string path = "blah.txt";
            try
            {
                try
                {
                    string text = File.ReadAllText(path);
                }
                catch (Exception ioex)
                {
                    throw new ApplicationException("Error reading file.", ioex);
                }
            }
            catch (Exception ex)
            {
                var log = Log.Error()
                    .Message("Error reading file '{0}'.", path)
                    .Exception(ex)
                    .Property("Test", "ErrorWrite")
                    .LogEventInfo;

                logEvent.Populate(log);
            }

            string fileName = string.Format("LogEvent-{0}.xml", DateTime.Now.Ticks);

            string xml = logEvent.Save();
            File.WriteAllText(fileName, xml);

            string outputFileName = Path.ChangeExtension(fileName, ".Writer.xml");

            var settings = new XmlWriterSettings { Indent = true };
            var writer = XmlWriter.Create(outputFileName, settings);
            var eventWriter = new LogEventWriter();

            eventWriter.Write(writer, logEvent);
            writer.Flush();
            writer.Close();

            string newXml = File.ReadAllText(outputFileName);
            var newEvent = LogEvent.Load(newXml);
            newEvent.Should().NotBeNull();
        }
    }
}

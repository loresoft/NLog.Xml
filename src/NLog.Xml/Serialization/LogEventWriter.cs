using System;
using System.Collections.Generic;
using System.Xml;
using NLog.Model;

namespace NLog.Serialization
{
    /// <summary>
    /// <see cref="LogEvent"/> XML writer
    /// </summary>
    public class LogEventWriter
    {
        /// <summary>
        /// Writes the specified <paramref name="logEvent"/> to XML
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to write to</param>
        /// <param name="logEvent">The log event to write</param>
        public void Write(XmlWriter writer, LogEvent logEvent)
        {
            writer.WriteStartElement("LogEvent");

            if (logEvent.SequenceID != default(int))
                writer.WriteElementString("SequenceID", 
                    XmlConvert.ToString(logEvent.SequenceID));

            if (logEvent.TimeStamp != default(DateTime))
                writer.WriteElementString("TimeStamp", XmlConvert.ToString(logEvent.TimeStamp, XmlDateTimeSerializationMode.RoundtripKind));

            if (logEvent.Level != null)
                writer.WriteElementString("Level", logEvent.Level);

            if (logEvent.LoggerName != null)
                writer.WriteElementString("LoggerName", logEvent.LoggerName);

            if (logEvent.Message != null)
                writer.WriteElementString("Message", logEvent.Message);

            if (logEvent.StackTrace != null)
                writer.WriteElementString("StackTrace", logEvent.StackTrace);


            if (logEvent.Error != null)
            {
                writer.WriteStartElement("Error");
                WriteError(writer, logEvent.Error);
                writer.WriteEndElement(); // Error 
            }

            if (logEvent.Properties != null)
            {
                writer.WriteStartElement("Properties");
                WriteProperties(writer, logEvent.Properties);
                writer.WriteEndElement(); // Properties 
            }

            writer.WriteEndElement(); // LogEvent
        }

        private void WriteError(XmlWriter writer, Error error)
        {
            if (error == null)
                return;

            if (error.TypeName != null)
                writer.WriteElementString("TypeName", error.TypeName);

            if (error.ErrorCode != null)
                writer.WriteElementString("ErrorCode", error.ErrorCode);

            if (error.MethodName != null)
                writer.WriteElementString("MethodName", error.MethodName);

            if (error.ModuleName != null)
                writer.WriteElementString("ModuleName", error.ModuleName);

            if (error.ModuleVersion != null)
                writer.WriteElementString("ModuleVersion", error.ModuleVersion);

            if (error.Message != null)
                writer.WriteElementString("Message", error.Message);

            if (error.Source != null)
                writer.WriteElementString("Source", error.Source);

            if (error.StackTrace != null)
                writer.WriteElementString("StackTrace", error.StackTrace);

            if (error.ExceptionText != null)
                writer.WriteElementString("ExceptionText", error.ExceptionText);

            if (error.InnerError != null)
            {
                writer.WriteStartElement("InnerError");
                WriteError(writer, error.InnerError);
                writer.WriteEndElement(); // InnerError
            }
        }

        private void WriteProperties(XmlWriter writer, IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                writer.WriteStartElement("Property");
                WriteProperty(writer, property);
                writer.WriteEndElement(); // Property 
            }
        }

        private void WriteProperty(XmlWriter writer, Property property)
        {
            if (property.Name != null)
                writer.WriteElementString("Name", property.Name);

            if (property.Value != null)
                writer.WriteElementString("Value", property.Value);
        }
    }
}

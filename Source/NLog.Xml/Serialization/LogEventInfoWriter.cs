using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace NLog.Serialization
{
    public class LogEventInfoWriter
    {
        public void Write(StringBuilder builder, LogEventInfo logEventInfo)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            using (var xw = XmlWriter.Create(builder, settings))
            {
                Write(xw, logEventInfo);
                xw.Flush();
            }
        }

        public void Write(XmlWriter writer, LogEventInfo logEvent)
        {
            writer.WriteStartElement("LogEvent");

            if (logEvent.SequenceID != default(int))
                writer.WriteElementString("SequenceID", XmlConvert.ToString(logEvent.SequenceID));

            if (logEvent.TimeStamp != default(DateTime))
                writer.WriteElementString("TimeStamp", XmlConvert.ToString(logEvent.TimeStamp, XmlDateTimeSerializationMode.RoundtripKind));

            if (logEvent.Level != null)
                writer.WriteElementString("Level", logEvent.Level.Name);

            if (logEvent.LoggerName != null)
                writer.WriteElementString("LoggerName", logEvent.LoggerName);

            if (logEvent.Message != null)
                writer.WriteElementString("Message", logEvent.FormattedMessage);

            if (logEvent.Exception != null)
            {
                writer.WriteStartElement("Error");
                WriteError(writer, logEvent.Exception);
                writer.WriteEndElement(); // Error 
            }


#if !SILVERLIGHT

            if (logEvent.HasStackTrace)
                writer.WriteElementString("StackTrace", logEvent.StackTrace.ToString());

#endif

            if (logEvent.Properties != null)
            {
                writer.WriteStartElement("Properties");
                WriteProperties(writer, logEvent.Properties);
                writer.WriteEndElement(); // Properties 
            }

            writer.WriteEndElement(); // LogEvent
        }

        private void WriteError(XmlWriter writer, Exception error)
        {
            if (error == null)
                return;

            Type type = error.GetType();
            var typeName = type.FullName;

            writer.WriteElementString("TypeName", typeName);

#if !SILVERLIGHT
            var external = error as ExternalException;
            if (external != null)
                writer.WriteElementString("ErrorCode", external.ErrorCode.ToString(CultureInfo.InvariantCulture));

            var method = error.TargetSite;
            if (method != null)
            {
                var assembly = method.Module.Assembly.GetName();

                writer.WriteElementString("MethodName", method.Name);
                if (assembly.Name != null)
                    writer.WriteElementString("ModuleName", assembly.Name);

                writer.WriteElementString("ModuleVersion", assembly.Version.ToString());

            }
#endif

            writer.WriteElementString("Message", error.Message);

#if !SILVERLIGHT
            if (error.Source != null)
                writer.WriteElementString("Source", error.Source);
#endif

            writer.WriteElementString("StackTrace", error.StackTrace);
            writer.WriteElementString("ExceptionText", error.ToString());

            if (error.InnerException == null)
                return;

            writer.WriteStartElement("InnerError");
            WriteError(writer, error.InnerException);
            writer.WriteEndElement(); // InnerError
        }

        private void WriteProperties(XmlWriter writer, IDictionary<object, object> properties)
        {
            foreach (var property in properties)
            {
                writer.WriteStartElement("Property");
                WriteProperty(writer, property);
                writer.WriteEndElement(); // Property 
            }
        }

        private void WriteProperty(XmlWriter writer, KeyValuePair<object, object> property)
        {
            var name = Convert.ToString(property.Key, CultureInfo.InvariantCulture);
            var value = Convert.ToString(property.Value, CultureInfo.InvariantCulture);

            writer.WriteElementString("Name", name);
            writer.WriteElementString("Value", value);
        }

    }
}
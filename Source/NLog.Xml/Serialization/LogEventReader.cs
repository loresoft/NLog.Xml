using System;
using System.Collections.Generic;
using System.Xml;
using NLog.Model;

namespace NLog.Serialization
{
    public class LogEventReader
    {
        public LogEvent Read(XmlReader reader)
        {
            LogEvent logEvent = null;

            reader.MoveToContent();

            if (reader.NodeType == XmlNodeType.Element)
                if (reader.LocalName == "LogEvent")
                    logEvent = ReadLog(reader);

            return logEvent;
        }

        private LogEvent ReadLog(XmlReader reader)
        {
            var logEvent = new LogEvent();

            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return logEvent;
            }

            reader.ReadStartElement();
            reader.MoveToContent();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    continue;
                }

                string name = reader.LocalName;
                if (name == "SequenceID")
                    logEvent.SequenceID = XmlConvert.ToInt32(reader.ReadElementString());
                else if (name == "TimeStamp")
                    logEvent.TimeStamp = XmlConvert.ToDateTime(reader.ReadElementString(), XmlDateTimeSerializationMode.RoundtripKind);
                else if (name == "Level")
                    logEvent.Level = reader.ReadElementString();
                else if (name == "LoggerName")
                    logEvent.LoggerName = reader.ReadElementString();
                else if (name == "Message")
                    logEvent.Message = reader.ReadElementString();
                else if (name == "StackTrace")
                    logEvent.StackTrace = reader.ReadElementString();
                else if (name == "Error")
                    logEvent.Error = ReadError(reader);
                else if (name == "Properties")
                    logEvent.Properties = ReadProperties(reader);
                else
                    reader.Skip(); // skip unknown

                reader.MoveToContent();
            }

            reader.ReadEndElement();
            return logEvent;
        }

        private Error ReadError(XmlReader reader)
        {
            var error = new Error();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return error;
            }

            reader.ReadStartElement();
            reader.MoveToContent();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    continue;
                }

                string name = reader.LocalName;
                if (name == "TypeName")
                    error.TypeName = reader.ReadElementString();
                else if (name == "ErrorCode")
                    error.ErrorCode = reader.ReadElementString();
                else if (name == "MethodName")
                    error.MethodName = reader.ReadElementString();
                else if (name == "ModuleName")
                    error.ModuleName = reader.ReadElementString();
                else if (name == "ModuleVersion")
                    error.ModuleVersion = reader.ReadElementString();
                else if (name == "Message")
                    error.Message = reader.ReadElementString();
                else if (name == "Source")
                    error.Source = reader.ReadElementString();
                else if (name == "StackTrace")
                    error.StackTrace = reader.ReadElementString();
                else if (name == "ExceptionText")
                    error.ExceptionText = reader.ReadElementString();
                else if (name == "InnerError")
                    error.InnerError = ReadError(reader);
                else
                    reader.Skip(); // skip unknown

                reader.MoveToContent();
            }

            reader.ReadEndElement();
            return error;
        }

        private List<Property> ReadProperties(XmlReader reader)
        {
            var properties = new List<Property>();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return properties;
            }

            reader.ReadStartElement();
            reader.MoveToContent();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    continue;
                }

                string name = reader.LocalName;
                if (name == "Property")
                    properties.Add(ReadProperty(reader));
                else
                    reader.Skip(); // skip unknown

                reader.MoveToContent();
            }

            reader.ReadEndElement();
            return properties;
        }

        private Property ReadProperty(XmlReader reader)
        {
            var property = new Property();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return property;
            }

            reader.ReadStartElement();
            reader.MoveToContent();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    continue;
                }

                string name = reader.LocalName;
                if (name == "Name")
                    property.Name = reader.ReadElementString();
                else if (name == "Value")
                    property.Value = reader.ReadElementString();
                else
                    reader.Skip(); // skip unknown

                reader.MoveToContent();
            }

            reader.ReadEndElement();
            return property;
        }
    }
}

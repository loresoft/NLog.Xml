using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NLog.Serialization;

namespace NLog.Model
{
    /// <summary>
    /// A log event data transfer object.
    /// </summary>
    [DebuggerDisplay("Logger: '{LoggerName}' Level: {Level} Message: '{Message}'")]
    public class LogEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEvent"/> class.
        /// </summary>
        public LogEvent()
        {
            Properties = new List<Property>();
        }

        /// <summary>
        /// Gets or sets the sequence ID.
        /// </summary>
        /// <value>
        /// The sequence ID.
        /// </value>
        public int SequenceID { get; set; }
        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public string Level { get; set; }
        /// <summary>
        /// Gets or sets the name of the logger.
        /// </summary>
        /// <value>
        /// The name of the logger.
        /// </value>
        public string LoggerName { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public Error Error { get; set; }
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Populates from specified <see cref="LogEventInfo"/>.
        /// </summary>
        /// <param name="logEventInfo">The log event info.</param>
        public void Populate(LogEventInfo logEventInfo)
        {
            SequenceID = logEventInfo.SequenceID;
            TimeStamp = logEventInfo.TimeStamp;

            Level = logEventInfo.Level != null
                ? logEventInfo.Level.Name
                : LogLevel.Debug.Name;

            LoggerName = logEventInfo.LoggerName;
            Message = logEventInfo.FormattedMessage;
            
            // will return null if Exception is null
            Error = Error.Create(logEventInfo.Exception);

#if !SILVERLIGHT
            if (logEventInfo.HasStackTrace)
                StackTrace = logEventInfo.StackTrace.ToString();

            var machineName = new Property { Name = "MachineName", Value = Environment.MachineName };
            Properties.Add(machineName);
#endif

            if (logEventInfo.Properties == null || logEventInfo.Properties.Count == 0)
                return;

            var properties = logEventInfo.Properties.Select(pair => new Property
            {
                Name = Convert.ToString(pair.Key, CultureInfo.InvariantCulture),
                Value = Convert.ToString(pair.Value, CultureInfo.InvariantCulture)
            });

            Properties.AddRange(properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Log Event: Logger='{LoggerName}' Level={Level} Message='{Message}'";
        }

        #region Serialize

        /// <summary>Saves this instance to an XML string.</summary>
        /// <returns>An XML string representing this instance.</returns>
        public string Save()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            // make sure xml is in utf-8
            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, settings))
                {
                    Save(xw);
                }

                // move to begining
                ms.Seek(0, SeekOrigin.Begin);

                // convert stream to utf-8 text
                using (var sr = new StreamReader(ms, Encoding.UTF8, true))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>Saves this instance the specified <paramref name="stream"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to save this instance to.</param>
        public void Save(Stream stream)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            using (var xw = XmlWriter.Create(stream, settings))
            {
                Save(xw);
            }
        }

        /// <summary>Saves this instance the specified <paramref name="writer"/>.</summary>
        /// <param name="writer">The <see cref="TextWriter"/> to save this instance to.</param>
        public void Save(TextWriter writer)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            using (var xw = XmlWriter.Create(writer, settings))
            {
                Save(xw);
            }
        }

        /// <summary>Saves this instance the specified <paramref name="writer"/>.</summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to save this instance to.</param>
        public void Save(XmlWriter writer)
        {
            var logWriter = new LogEventWriter();
            logWriter.Write(writer, this);

            writer.Flush();
        }

        /// <summary>Creates an new instance by loading the specified XML.</summary>
        /// <param name="xml">The XML string representing this instance.</param>
        /// <returns>A new instance deserialize from the specifed XML.</returns>
        public static LogEvent Load(string xml)
        {
            using (var sr = new StringReader(xml))
            using (var xr = XmlReader.Create(sr))
            {
                return Load(xr);
            }
        }

        /// <summary>Creates an new instance by loading the specified stream.</summary>
        /// <param name="stream">The <see cref="Stream"/> to load the instance from.</param>
        /// <returns>A new instance deserialize from the specifed reader.</returns>
        public static LogEvent Load(Stream stream)
        {
            using (var xr = XmlReader.Create(stream))
            {
                return Load(xr);
            }
        }

        /// <summary>Creates an new instance by loading the specified reader.</summary>
        /// <param name="reader">The reader to load the instance from.</param>
        /// <returns>A new instance deserialize from the specifed reader.</returns>
        public static LogEvent Load(TextReader reader)
        {
            using (var xr = XmlReader.Create(reader))
            {
                return Load(xr);
            }
        }

        /// <summary>Creates an new instance by loading the specified reader.</summary>
        /// <param name="reader">The reader to load the instance from.</param>
        /// <returns>A new instance deserialize from the specifed reader.</returns>
        public static LogEvent Load(XmlReader reader)
        {
            var logReader = new LogEventReader();
            var instance = logReader.Read(reader);
            return instance;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Model;
using NLog.Serialization;

namespace NLog.LayoutRenderers
{
    /// <summary>
    /// An Xml Layout Renderer for NLog.
    /// </summary>
    [LayoutRenderer("xml")]
    public class XmlLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Appends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="logEventInfo">The log event info.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEventInfo)
        {
            var writer = new LogEventInfoWriter();
            writer.Write(builder, logEventInfo);
        }
    }
}

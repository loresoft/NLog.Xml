using System;
using System.Diagnostics;

namespace NLog.Model
{
    /// <summary>
    /// A property data transfer object.
    /// </summary>
    [DebuggerDisplay("Name: {Name} Value: {Value}")]
    public class Property
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}
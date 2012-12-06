using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NLog.Targets
{
    [Target("LimitedMemory")]
    public sealed class LimitedMemoryTarget : TargetWithLayout
    {
        private readonly ConcurrentQueue<string> _logs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedMemoryTarget" /> class.
        /// </summary>
        /// <remarks>
        /// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
        /// </remarks>
        public LimitedMemoryTarget()
        {
            Limit = 100;
            _logs = new ConcurrentQueue<string>();
        }

        /// <summary>
        /// Gets or sets the number of messages to keep in memory.
        /// </summary>
        [DefaultValue(100)]
        public int Limit { get; set; }

        /// <summary>
        /// Gets the list of logs gathered in the <see cref="LimitedMemoryTarget"/>.
        /// </summary>
        public IEnumerable<string> Logs
        {
            get { return _logs; }
        }

        /// <summary>
        /// Renders the logging event message and adds it to the internal ArrayList of log messages.
        /// </summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            string message = Layout.Render(logEvent);
            _logs.Enqueue(message);

            while (_logs.Count > Limit)
            {
                string result;
                _logs.TryDequeue(out result);
            }
        }

    }
}

using System.ComponentModel;
using System.Messaging;
using System.Text;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;


namespace NLog.Xml.Targets
{
    /// <summary>
    /// Writes log message to the specified message queue handled by MSMQ.
    /// </summary>
    /// <seealso href="http://nlog-project.org/wiki/MessageQueue_target">Documentation on NLog Wiki</seealso>
    [Target("MessageQueue")]
    public class MessageQueueTarget : TargetWithLayout
    {
        private const MessagePriority _messagePriority = MessagePriority.Normal;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueTarget"/> class.
        /// </summary>
        /// <remarks>
        /// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
        /// </remarks>
        public MessageQueueTarget()
        {
            this.Label = "NLog";
            this.Encoding = System.Text.Encoding.UTF8;
        }

        /// <summary>
        /// Gets or sets the name of the queue to write to.
        /// </summary>
        /// <remarks>
        /// To write to a private queue on a local machine use <c>.\private$\QueueName</c>.
        /// For other available queue names, consult MSMQ documentation.
        /// </remarks>
        /// <docgen category='Queue Options' order='10' />
        [RequiredParameter]
        public Layout Queue { get; set; }

        /// <summary>
        /// Gets or sets the label to associate with each message.
        /// </summary>
        /// <remarks>
        /// By default no label is associated.
        /// </remarks>
        /// <docgen category='Queue Options' order='10' />
        [DefaultValue("NLog")]
        public Layout Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create the queue if it doesn't exists.
        /// </summary>
        /// <docgen category='Queue Options' order='10' />
        [DefaultValue(false)]
        public bool CreateQueueIfNotExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use recoverable messages (with guaranteed delivery).
        /// </summary>
        /// <docgen category='Queue Options' order='10' />
        [DefaultValue(false)]
        public bool Recoverable { get; set; }

        /// <summary>
        /// Gets or sets the encoding to be used when writing text to the queue.
        /// </summary>
        /// <docgen category='Layout Options' order='10' />
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the XML format when serializing message.
        /// </summary>
        /// <docgen category='Layout Options' order='10' />
        [DefaultValue(false)]
        public bool UseXmlEncoding { get; set; }

        /// <summary>
        /// Writes the specified logging event to a queue specified in the Queue 
        /// parameter.
        /// </summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            if (this.Queue == null)
                return;

            string queue = this.Queue.Render(logEvent);

            if (CreateQueueIfNotExists && !MessageQueue.Exists(queue))
                MessageQueue.Create(queue);

            Message prepareMessage = this.PrepareMessage(logEvent);
            if (prepareMessage == null)
                return;

            using (var messageQueue = new MessageQueue(queue))
                messageQueue.Send(prepareMessage);
        }

        /// <summary>
        /// Prepares a message to be sent to the message queue.
        /// </summary>
        /// <param name="logEvent">The log event to be used when calculating label and text to be written.</param>
        /// <returns>The message to be sent.</returns>
        /// <remarks>
        /// You may override this method in inheriting classes
        /// to provide services like encryption or message 
        /// authentication.
        /// </remarks>
        protected virtual Message PrepareMessage(LogEventInfo logEvent)
        {
            var message = new Message();
            if (this.Label != null)
                message.Label = this.Label.Render(logEvent);

            message.Recoverable = this.Recoverable;
            message.Priority = _messagePriority;

            string body = Layout.Render(logEvent);

            if (UseXmlEncoding)
            {
                message.Body = body;
                return message;
            }

            byte[] dataBytes = this.Encoding.GetBytes(body);
            message.BodyStream.Write(dataBytes, 0, dataBytes.Length);

            return message;
        }
    }
}

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Layouts;
using NLog.Xml.Targets;

namespace NLog.Xml.Test
{
    [TestClass]
    public class MessageQueueTargetTest
    {
        [TestMethod]
        public void Write()
        {
            var logEvent = new NLog.LogEventInfo(LogLevel.Debug, "Test.Logger", "This is a test message.");

            var asyncLogEventInfo = new NLog.Common.AsyncLogEventInfo(logEvent, ex =>
            {
                if (ex != null) Console.WriteLine(ex);
            });
            
            var target = new MessageQueueTarget();
            target.CreateQueueIfNotExists = true;
            target.Label = new SimpleLayout("${logger}");
            target.Layout = new XmlLayout();
            target.Name = "messageQueue";
            target.Queue = new SimpleLayout(@".\private$\logging");
            target.Recoverable = true;

            target.WriteAsyncLogEvent(asyncLogEventInfo);

            Thread.Sleep(5000);
        }
    }
}

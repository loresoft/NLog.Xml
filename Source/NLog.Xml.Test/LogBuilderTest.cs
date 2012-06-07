using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Fluent;

namespace NLog.Xml.Test
{
    [TestClass]
    public class LogBuilderTest
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [TestMethod]
        public void TraceWrite()
        {
            _logger.Trace()
                .Message("This is a test fluent message.")
                .Property("Test", "TraceWrite")
                .Write();

            _logger.Trace()
                .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
                .Property("Test", "TraceWrite")
                .Write();
        }

        [TestMethod]
        public void InfoWrite()
        {
            _logger.Info()
                .Message("This is a test fluent message.")
                .Property("Test", "InfoWrite")
                .Write();

            _logger.Info()
                .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
                .Property("Test", "InfoWrite")
                .Write();
        }

        [TestMethod]
        public void ErrorWrite()
        {
            string path = "blah.txt";
            try
            {
                string text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                _logger.Error()
                    .Message("Error reading file '{0}'.", path)
                    .Exception(ex)
                    .Property("Test", "ErrorWrite")
                    .Write();
            }

            _logger.Error()
                .Message("This is a test fluent message.")
                .Property("Test", "ErrorWrite")
                .Write();

            _logger.Error()
                .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
                .Property("Test", "ErrorWrite")
                .Write();
        }

    }
}

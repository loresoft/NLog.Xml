using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Fluent;

namespace NLog.Xml.Test
{
    [TestClass]
    public class LogTest    
    {
        [TestMethod]
        public void TraceWrite()
        {
            Log.Trace()
                .Message("This is a test fluent message.")
                .Property("Test", "TraceWrite")
                .Write();

            Log.Trace()
                .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
                .Property("Test", "TraceWrite")
                .Write();
        }

        [TestMethod]
        public void InfoWrite()
        {
            Log.Info()
                .Message("This is a test fluent message.")
                .Property("Test", "InfoWrite")
                .Write();

            Log.Info()
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
                Log.Error()
                    .Message("Error reading file '{0}'.", path)
                    .Exception(ex)
                    .Property("Test", "ErrorWrite")
                    .Write();
            }

            Log.Error()
                .Message("This is a test fluent message.")
                .Property("Test", "ErrorWrite")
                .Write();

            Log.Error()
                .Message("This is a test fluent message '{0}'.", DateTime.Now.Ticks)
                .Property("Test", "ErrorWrite")
                .Write();
        }
    }
}

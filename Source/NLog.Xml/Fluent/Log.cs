using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace NLog.Fluent
{
    /// <summary>
    /// A global logging class using the Visual Studio 2012 caller info to find the logger.
    /// </summary>
    public static class Log
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Starts building a log event with the specified <see cref="LogLevel" />.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Level(LogLevel logLevel, [CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(logLevel, callerMemberName, callerFilePath, callerLineNumber);
        }
        
        /// <summary>
        /// Starts building a log event at the <c>Trace</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Trace([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Trace, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// Starts building a log event at the <c>Debug</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Debug([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Debug, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// Starts building a log event at the <c>Info</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Info([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Info, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// Starts building a log event at the <c>Warn</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Warn([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Warn, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// Starts building a log event at the <c>Error</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Error([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Error, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// Starts building a log event at the <c>Fatal</c> level.
        /// </summary>
        /// <param name="callerMemberName">The method or property name of the caller to the method.</param>
        /// <param name="callerFilePath">The full path of the source file that contains the caller. This is the file path at the time of compile.</param>
        /// <param name="callerLineNumber">The line number in the source file at which the method is called.</param>
        /// <returns>An instance of the fluent <see cref="LogBuilder"/>.</returns>
        public static LogBuilder Fatal([CallerMemberName]string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber]int callerLineNumber = 0)
        {
            return Create(LogLevel.Fatal, callerMemberName, callerFilePath, callerLineNumber);
        }

        private static LogBuilder Create(LogLevel logLevel, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            string name = Path.GetFileNameWithoutExtension(callerFilePath ?? string.Empty);
            var logger = string.IsNullOrWhiteSpace(name) ? _logger : LogManager.GetLogger(name);

            var builder = new LogBuilder(logger, logLevel);

            builder
                .Property("CallerMemberName", callerMemberName)
                .Property("CallerFilePath", callerFilePath)
                .Property("CallerLineNumber", callerLineNumber);

            return builder;
        }
    }
}

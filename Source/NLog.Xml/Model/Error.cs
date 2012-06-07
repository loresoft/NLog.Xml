using System;
using System.Diagnostics;
using System.Reflection;

namespace NLog.Model
{
    /// <summary>
    /// An error data transfer object.
    /// </summary>
    [DebuggerDisplay("Type: '{TypeName}' Message: '{Message}'")]
    public class Error
    {
        /// <summary>
        /// Gets or sets the exception type name.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the module version.
        /// </summary>
        /// <value>
        /// The module version.
        /// </value>
        public string ModuleVersion { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the exception text value.
        /// </summary>
        /// <value>
        /// The exception text.
        /// </value>
        public string ExceptionText { get; set; }

        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        /// <value>
        /// The inner exception.
        /// </value>
        public Error InnerError { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ExceptionText;
        }

        /// <summary>
        /// Gets the base exception detail.
        /// </summary>
        /// <returns></returns>
        public Error GetBaseError()
        {
            Error innerException = InnerError;
            Error parentException = this;
            while (innerException != null)
            {
                parentException = innerException;
                innerException = innerException.InnerError;
            }

            return parentException;
        }

        /// <summary>
        /// Populates with the specified <see cref="Exception"/>.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to create from.</param>
        public void Populate(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            Message = ex.Message;
            ExceptionText = ex.ToString();
            StackTrace = ex.StackTrace;

            Type type = ex.GetType();
            TypeName = type.FullName;

#if !SILVERLIGHT
            PropertyInfo info = type.GetProperty("HResult", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
                ErrorCode = info.GetValue(ex, null).ToString();

            Source = ex.Source;

            MethodBase method = ex.TargetSite;
            if (method != null)
            {
                MethodName = method.Name;

                AssemblyName assembly = method.Module.Assembly.GetName();
                ModuleName = assembly.Name;
                ModuleVersion = assembly.Version.ToString();
            }
#endif

            if (ex.InnerException == null)
                return;

            InnerError = new Error();
            InnerError.Populate(ex.InnerException);
        }

        /// <summary>
        /// Creates with the specified <see cref="Exception"/>.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to create from.</param>
        /// <returns></returns>
        public static Error Create(Exception ex)
        {
            if (ex == null)
                return null;

            var ed = new Error();
            ed.Populate(ex);
            return ed;
        }
    }

}

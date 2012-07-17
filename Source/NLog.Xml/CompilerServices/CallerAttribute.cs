using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !net45
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Allows you to obtain the full path of the source file that contains the caller. This is the file path at the time of compile.
    /// </summary>
    /// <remarks>
    /// This attribute is included to provide support for targeting .net 4.0 framework.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerFilePathAttribute : Attribute
    {
    }

    /// <summary>
    /// Allows you to obtain the line number in the source file at which the method is called.
    /// </summary>
    /// <remarks>
    /// This attribute is included to provide support for targeting .net 4.0 framework.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerLineNumberAttribute : Attribute
    {
    }

    /// <summary>
    /// Allows you to obtain the method or property name of the caller to the method.
    /// </summary>
    /// <remarks>
    /// This attribute is included to provide support for targeting .net 4.0 framework.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }
}
#endif

using System;

namespace CMDotNet
{
    /// <summary>
    /// Indicates that a parameter should be treated as a remainder.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RemainderAttribute : Attribute
    {
    }
}
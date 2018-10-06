using System;

namespace CMDotNet
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RemainderAttribute : Attribute
    {
    }
}
using System;

namespace CMDotNet
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
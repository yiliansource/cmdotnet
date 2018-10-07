using System;

namespace CMDotNet
{
    /// <summary>
    /// Indicates that a method can be invoked as a command.
    /// </summary>
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
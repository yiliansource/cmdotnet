using System;

namespace CMDotNet
{
    /// <summary>
    /// Indicates aliases for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AliasAttribute : Attribute
    {
        public AliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
        public string[] Aliases { get; }
    }
}

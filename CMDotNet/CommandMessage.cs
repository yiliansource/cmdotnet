using System;

namespace CMDotNet
{
    public class CommandMessage
    {
        public string Content { get; set; }

        public CommandMessage(string content)
        {
            Content = content;
        }

        public bool HasStringPrefix(string prefix, ref int argPos)
        {
            if (Content.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                argPos = prefix.Length;
                return true;
            }
            return false;
        }
    }
}

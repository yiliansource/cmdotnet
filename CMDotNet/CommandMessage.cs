using System;

namespace CMDotNet
{
    /// <summary>
    /// Represents a message that can be used to invoke a command.
    /// </summary>
    public class CommandMessage : IMessage
    {
        /// <summary>
        /// The raw content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Creates a new command message, based on the specified content.
        /// </summary>
        public CommandMessage(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Checks if the message has a specific string prefixed to it. If it does, <paramref name="argPos"/> will indicate the position where the prefix ends.
        /// </summary>
        public bool HasStringPrefix(string prefix, ref int argPos)
        {
            if (Content.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                argPos = prefix.Length;
                return true;
            }
            argPos = 0;
            return false;
        }

        /// <summary>
        /// Returns the content of the message.
        /// </summary>
        public override string ToString()
            => Content;
    }
}

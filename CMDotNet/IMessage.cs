using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDotNet
{
    /// <summary>
    /// Base for a message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The content for a message.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Checks if the message has a specific string prefixed to it. If it does, <paramref name="argPos"/> will indicate the position where the prefix ends.
        /// </summary>
        bool HasStringPrefix(string prefix, ref int argPos);
    }
}

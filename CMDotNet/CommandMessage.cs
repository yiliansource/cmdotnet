using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

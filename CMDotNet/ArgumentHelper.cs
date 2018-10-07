using System.Linq;

namespace CMDotNet
{
    /// <summary>
    /// Provides utility to deal with commands and arguments.
    /// </summary>
    public static class ArgumentHelper
    {
        /// <summary>
        /// Extracts the name and arguments of a command, contained in a message. Also skips until <paramref name="argPos"/>, in case the command has a prefix.
        /// </summary>
        public static void ExtractNameAndArgs(IMessage msg, int argPos, out string name, out string[] args)
        {
            string input = msg.Content.Substring(argPos);
            string[] inputParts = input.Split(' ');

            name = inputParts[0];
            args = inputParts.Skip(1).ToArray();
        }
    }
}

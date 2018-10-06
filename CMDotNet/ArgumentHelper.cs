using System.Linq;

namespace CMDotNet
{
    public static class ArgumentHelper
    {
        public static void ExtractNameAndArgs(CommandMessage msg, int argPos, out string name, out string[] args)
        {
            string input = msg.Content.Substring(argPos);
            string[] inputParts = msg.Content.Split(' ');

            name = inputParts[0];
            args = inputParts.Skip(1).ToArray();
        }
    }
}

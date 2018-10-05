using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CMDotNet.UnitTests
{
    public class ParseTestModule : CommandModule
    {
        [Command("ping")]
        public void Ping()
        {
            Console.WriteLine("Pong!");
        }

        [Command("square")]
        public void Square(int num)
        {
            Console.WriteLine($"Squared Value of { num }: { num * num }");
        }

        [Command("write")]
        public void Write([Remainder]string text)
        {
            Console.WriteLine(text);
        }
    }
}

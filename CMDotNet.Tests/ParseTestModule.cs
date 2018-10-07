using System;

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

        [Command("mynum")]
        public void MyNumTest(MyNum num)
        {
            Console.WriteLine($"The number is: { num.Num }!");
        }

        [Command("aliasedcommand"), Alias("alias")]
        public void AliasTest()
        {
            Console.WriteLine("This was an alias command!");
        }
    }
}

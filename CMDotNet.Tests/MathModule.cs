using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSource.Commands.UnitTests
{
    public class MathModule : CommandModule
    {
        [Command("add")]
        public void Add(int num1, int num2)
        {
            Console.WriteLine(num1 + num2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSource.Commands.UnitTests
{
    public class ConsoleCommandHost : CommandHost
    {
        

        public void HandleCommand(string command)
        {
            string[] parts = command.Split(' ');
            Executer.ExecuteCommand(parts.First(), parts.Skip(1).ToArray());
        }
    }
}

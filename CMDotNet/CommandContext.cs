using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CMDotNet
{
    /// <summary>
    /// Stores context for the execution of a command.
    /// </summary>
    public class CommandContext
    {
        public TextWriter Out { get; set; }
    }
}

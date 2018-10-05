using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDotNet
{
    public interface IExecutionResult
    {
        CommandError? Error { get; }
        string ErrorReason { get; }
        bool IsSuccess { get; }
    }
}

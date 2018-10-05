using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDotNet
{
    public class ExecutionResult : IExecutionResult
    {
        public CommandError? Error { get; private set; }
        public string ErrorReason { get; private set; }
        public bool IsSuccess => Error == null;

        public static ExecutionResult FromError(CommandError? error, string reason)
            => new ExecutionResult { Error = error, ErrorReason = reason };
        public static ExecutionResult Success
            => new ExecutionResult();
    }
}

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

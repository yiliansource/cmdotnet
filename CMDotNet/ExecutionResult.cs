namespace CMDotNet
{
    /// <summary>
    /// The result of a command execuion.
    /// </summary>
    public class ExecutionResult : IExecutionResult
    {
        /// <summary>
        /// If the command threw an error, this will be the error type.
        /// </summary>
        public CommandError? Error { get; private set; }
        /// <summary>
        /// The detailed reason why the command failed.
        /// </summary>
        public string ErrorReason { get; private set; }
        /// <summary>
        /// Checks if the execution result indicates success.
        /// </summary>
        public bool IsSuccess => Error == null;

        /// <summary>
        /// Builds an <see cref="ExecutionResult"/> from an error and a reason.
        /// </summary>
        public static ExecutionResult FromError(CommandError? error, string reason)
            => new ExecutionResult { Error = error, ErrorReason = reason };
        /// <summary>
        /// Returns a successful execution result.
        /// </summary>
        public static ExecutionResult Success
            => new ExecutionResult();
    }
}

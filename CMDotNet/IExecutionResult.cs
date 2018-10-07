namespace CMDotNet
{
    /// <summary>
    /// The base for an execution result.
    /// </summary>
    public interface IExecutionResult
    {
        /// <summary>
        /// If the command threw an error, this will be the error type.
        /// </summary>
        CommandError? Error { get; }
        /// <summary>
        /// The detailed reason why the command failed.
        /// </summary>
        string ErrorReason { get; }
        /// <summary>
        /// Checks if the execution result indicates success.
        /// </summary>
        bool IsSuccess { get; }
    }
}

namespace CMDotNet
{
    public interface IExecutionResult
    {
        CommandError? Error { get; }
        string ErrorReason { get; }
        bool IsSuccess { get; }
    }
}

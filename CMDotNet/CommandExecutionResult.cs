namespace YSource.Commands
{
    public class CommandExecutionResult
    {
        public string ErrorReason { get; set; }
        public CommandError Error { get; set; }

        public bool Success => Error == CommandError.None;

        public static implicit operator bool(CommandExecutionResult r)
            => r.Success;
    }
}

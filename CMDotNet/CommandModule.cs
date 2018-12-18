namespace CMDotNet
{
    /// <summary>
    /// The base class for a command module.
    /// </summary>
    public abstract class CommandModule
    {
        /// <summary>
        /// The context of the current command execution.
        /// </summary>
        public CommandContext Context { get; set; }
    }
}

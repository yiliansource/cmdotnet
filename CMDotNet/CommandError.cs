namespace CMDotNet
{
    /// <summary>
    /// The error that a command has caused.
    /// </summary>
    public enum CommandError
    {
        /// <summary>
        /// The command doesn't exist.
        /// </summary>
        UnknownCommand,
        /// <summary>
        /// The command had invalid parameters passed.
        /// </summary>
        InvalidParameters,
        /// <summary>
        /// Another exception occurred during command invokation.
        /// </summary>
        RuntimeException
    }
}

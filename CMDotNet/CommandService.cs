using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CMDotNet
{
    /// <summary>
    /// Represents a service that can be used to invoke commands.
    /// </summary>
    public class CommandService
    {
        /// <summary>
        /// The standard writer to output to. <see cref="Console.Out"/> by default.
        /// </summary>
        public TextWriter Out { get; set; } = Console.Out;

        private IReadOnlyCollection<CommandInfo> _commands;

        /// <summary>
        /// Creates a new instance of a command service.
        /// </summary>
        public CommandService() 
        {
            _commands = new CommandInfo[0];
        }

        /// <summary>
        /// Registers all modules in the given assembly to the service.
        /// </summary>
        public void AddModulesAsync(Assembly source)
        {
            _commands = _commands.Concat(
                    source.GetExportedTypes()
                    .Where(type => type.IsSubclassOf(typeof(CommandModule)))
                    .SelectMany(module => CommandInfo.FromModule(module)))
                .ToArray();
        }
        
        /// <summary>
        /// Executes a command, given by a message.
        /// </summary>
        /// <param name="msg">The message where the command is located.</param>
        /// <param name="argPos">The first character index after the prefix ends. 0 if there is no prefix.</param>
        public IExecutionResult Execute(IMessage msg, int argPos)
        {
            CommandContext defaultContext = new CommandContext
            {
                Out = Out
            };
            return Execute(msg, argPos, defaultContext);
        }
        /// <summary>
        /// Executes a command, given by a message.
        /// </summary>
        /// <param name="msg">The message where the command is located.</param>
        /// <param name="argPos">The first character index after the prefix ends. 0 if there is no prefix.</param>
        /// <param name="context">The context, in which the command is executed.</param>
        public IExecutionResult Execute(IMessage msg, int argPos, CommandContext context)
        {
            CommandInfo command = Search(msg, argPos);
            if (command == null)
                return ExecutionResult.FromError(CommandError.UnknownCommand, "The command doesn't exist or wasn't registered.");
            else
                return command.Execute(msg, argPos, context);
        }

        /// <summary>
        /// Searches the service for a command, by the data of a message.
        /// </summary>
        public CommandInfo Search(IMessage msg, int argPos)
        {
            ArgumentHelper.ExtractNameAndArgs(msg, argPos, out string name, out string[] args);

            // Check if any command name matches the input name.
            IEnumerable<CommandInfo> nameMatches = _commands.Where(command => string.Equals(command.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (nameMatches.Count() == 0)
            {
                // No matches were found? Try again with alias.
                nameMatches = _commands.Where(command => command.Aliases != null && command.Aliases.Any(alias => string.Equals(alias, name, StringComparison.InvariantCultureIgnoreCase)));
                if (nameMatches.Count() == 0)
                {
                    // No command was found.
                    return null;
                }
            }
            // Continue filtering with matching parameter count
            IEnumerable<CommandInfo> parameterMatches = nameMatches.Where(command => command.Parameters.Count == args.Length);
            if (parameterMatches.Count() == 0)
            {
                // No matches were found? Try again, this time regarding the [Remainder] attribute.
                parameterMatches = nameMatches.Where(command => (command.Parameters.Last().IsRemainder || command.Parameters.Last().IsParamsArray) && command.Parameters.Count <= args.Length);
                if (parameterMatches.Count() == 0)
                {
                    // No command was found.
                    return null;
                }
            }
            // Return the first match. Multiple matches should not be possible.
            return parameterMatches.First();
        }
        /// <summary>
        /// Returns all commands that have been registered into the service.
        /// </summary>
        public IEnumerable<CommandInfo> GetRegisteredCommands()
            => _commands;
    }
}

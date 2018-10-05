﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMDotNet
{
    public class CommandService
    {
        private IReadOnlyCollection<CommandInfo> _commands;

        public void AddModulesAsync(Assembly source)
        {
            _commands = source.GetExportedTypes()
                .Where(type => type.IsSubclassOf(typeof(CommandModule)))
                .SelectMany(module => CommandInfo.FromModule(module))
                .ToArray();
        }

        public IExecutionResult Execute(CommandMessage msg, int argPos)
        {
            CommandInfo command = Search(msg, argPos);
            if (command == null)
            {
                return ExecutionResult.FromError(CommandError.UnknownCommand, "The command doesn't exist or wasn't registered.");
            }
            else
            {
                return command.Execute(msg, argPos);
            }
        }

        public CommandInfo Search(CommandMessage msg, int argPos)
        {
            ArgumentHelper.ExtractNameAndArgs(msg, argPos, out string name, out string[] args);

            // Check if any command name matches the input name.
            IEnumerable<CommandInfo> nameMatches = _commands.Where(command => string.Equals(command.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (nameMatches.Count() == 0)
            {
                // No matches were found? Try again with alias.
                nameMatches = _commands.Where(command => command.Aliases.Any(alias => string.Equals(alias, name, StringComparison.InvariantCultureIgnoreCase)));
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
                parameterMatches = nameMatches.Where(command => command.Parameters.Last().IsRemainder && command.Parameters.Count <= args.Length);
                if (parameterMatches.Count() == 0)
                {
                    // No command was found.
                    return null;
                }
            }
            // Return the first match. Multiple matches should not be possible.
            return parameterMatches.First();
        }
    }
}
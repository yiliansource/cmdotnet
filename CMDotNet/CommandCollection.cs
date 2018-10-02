using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YSource.Commands
{
    public class CommandCollection : IEnumerable<Command>
    {
        public IEnumerable<Command> Commands { get; set; }

        public void Populate(IEnumerable<Type> modules)
        {
            Commands = modules
                .SelectMany(m => m.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)
                .Select(m =>
                {
                    CommandAttribute commandData = m.GetCustomAttribute<CommandAttribute>();
                    AliasAttribute aliasData = m.GetCustomAttribute<AliasAttribute>();

                    return new Command
                    {
                        Name = commandData.Name,
                        Aliases = aliasData?.Aliases ?? new string[0],
                        Attributes = m.GetCustomAttributes().ToArray(),
                        Parameters = m.GetParameters(),
                        Method = m
                    };
                });
        }

        public Command GetCommand(string name, int paramCount)
        {
            return Commands.FirstOrDefault(c =>
                c.Parameters.Length == paramCount
                && (string.Equals(name, c.Name, StringComparison.InvariantCultureIgnoreCase) || c.Aliases.Any(a => string.Equals(name, a, StringComparison.InvariantCultureIgnoreCase))));
        }

        public IEnumerator<Command> GetEnumerator()
            => Commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YSource.Commands
{
    public sealed class CommandExecuter
    {
        public IEnumerable<Type> InstalledModules { get; private set; }
        public CommandCollection Commands { get; set; }

        public void InstallModules(params Assembly[] assemblies)
        {
            Type commandModule = typeof(CommandModule);

            InstalledModules = assemblies
                .SelectMany(assembly => assembly.GetExportedTypes()
                    .Where(t => t.IsSubclassOf(commandModule) && !t.IsAbstract));

            Commands = new CommandCollection();
            Commands.Populate(InstalledModules);
        }

        public CommandExecutionResult ExecuteCommand(string name, params string[] inputParameters)
        {
            CommandExecutionResult result = new CommandExecutionResult();
            Command command = Commands.GetCommand(name, inputParameters.Length);

            if (command == null)
            {
                result.Error = CommandError.UnknownCommand;
                result.ErrorReason = "The specified command was not found in the modules.";
            }
            else
            {
                object[] parameters = null;
                try { parameters = inputParameters.Length == 0 ? new object[0] : GetParsedParameters(inputParameters, command.Parameters).ToArray(); }
                catch
                {
                    result.Error = CommandError.InvalidParameters;
                    result.ErrorReason = "Can't convert the parameters to the desired types.";
                }

                if (parameters != null)
                {
                    object source = Activator.CreateInstance(command.Method.DeclaringType);
                    try
                    {
                        command.Method.Invoke(source, parameters);
                        result.Error = CommandError.None;
                    }
                    catch (Exception ex)
                    {
                        result.Error = CommandError.RuntimeException;
                        result.ErrorReason = ex.ToString();
                    }
                }
            }
            return result;
        }

        private IEnumerable<object> GetParsedParameters(string[] input, ParameterInfo[] parameterInfos)
        {
            for (int i = 0; i < input.Length; i++)
                yield return Convert.ChangeType(input[i], parameterInfos[i].ParameterType);
        }
    }
}

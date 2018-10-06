using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CMDotNet
{
    public class CommandInfo
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Aliases { get; set; }
        public IReadOnlyCollection<ParameterInfo> Parameters { get; set; }
        public IReadOnlyCollection<Attribute> Attributes;

        private MethodInfo _method;

        private CommandInfo() { }
        public static CommandInfo FromMethod(MethodInfo method)
        {
            CommandAttribute commandData = method.GetCustomAttribute<CommandAttribute>();
            AliasAttribute aliasData = method.GetCustomAttribute<AliasAttribute>();

            if (commandData == null)
                throw new ArgumentException("The passed method isn't a command.");

            return new CommandInfo
            {
                Name = commandData.Name,
                Aliases = aliasData?.Aliases,
                Parameters = method.GetParameters()?.Select(parameter => ParameterInfo.FromParameter(parameter)).ToArray(),
                Attributes = method.GetCustomAttributes().ToArray(),

                _method = method
            };
        }
        public static CommandInfo[] FromModule(Type moduleType)
        {
            if (!moduleType.IsSubclassOf(typeof(CommandModule)))
                throw new ArgumentException("The passed type isn't a command module.");

            return moduleType
                .GetMethods()
                .Where(method => method.GetCustomAttribute<CommandAttribute>() != null)
                .Select(method => FromMethod(method))
                .ToArray();
        }

        public IExecutionResult Execute(CommandMessage msg, int argPos)
        {
            ArgumentHelper.ExtractNameAndArgs(msg, argPos, out string name, out string[] args);

            object[] parameters;
            try
            {
                if (Parameters.Count == 0)
                {
                    parameters = new object[0];
                }
                else
                {
                    if (Parameters.Last().IsRemainder)
                    {
                        int preRemainderLength = Parameters.Count - 1;
                        var preRemainderParameters = Parameters.Take(preRemainderLength);
                        var preRemainderArgs = args.Take(preRemainderLength);

                        var parsedPreRemainderParameters = preRemainderArgs.Zip(preRemainderParameters, (s, info) => info.ConvertToParameterType(s)).ToList();

                        var lastPar = Parameters.Last();
                        var remainder = string.Join(" ", args.Skip(preRemainderLength).ToArray());

                        parsedPreRemainderParameters.Add(lastPar.ConvertToParameterType(remainder));

                        parameters = parsedPreRemainderParameters.ToArray();
                    }
                    else
                    {
                        parameters = args.Zip(Parameters, (s, info) => info.ConvertToParameterType(s)).ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return ExecutionResult.FromError(CommandError.InvalidParameters, "Can't convert the parameters to the desired types.");
            }

            if (parameters != null)
            {
                object source = Activator.CreateInstance(_method.DeclaringType);
                try
                {
                    _method.Invoke(source, parameters);
                }
                catch (Exception ex)
                {
                    return ExecutionResult.FromError(CommandError.RuntimeException, ex.ToString());
                }
            }

            return ExecutionResult.Success;
        }
    }
}

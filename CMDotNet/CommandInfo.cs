using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CMDotNet
{
    /// <summary>
    /// Holds information about a command.
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The aliases of a command.
        /// </summary>
        public IReadOnlyCollection<string> Aliases { get; set; }
        /// <summary>
        /// The parameters of the command method.
        /// </summary>
        public IReadOnlyCollection<ParameterInfo> Parameters { get; set; }
        /// <summary>
        /// The attributes of the command.
        /// </summary>
        public IReadOnlyCollection<Attribute> Attributes;

        private MethodInfo _method;

        private CommandInfo() { }
        /// <summary>
        /// Creates a new <see cref="CommandInfo"/> from a <see cref="MethodInfo"/>.
        /// </summary>
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
        /// <summary>
        /// Extracts all the commands from a module of the specified type.
        /// </summary>
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

        private object[] ParseArgumentsToParameters(string[] args)
        {
            if (Parameters.Count == 0)
                return new object[0];
            else
            {
                var lastPar = Parameters.Last();
                if (lastPar.IsRemainder || lastPar.IsParamsArray)
                {
                    int preRemainderLength = Parameters.Count - 1;
                    var preRemainderParameters = Parameters.Take(preRemainderLength);
                    var preRemainderArgs = args.Take(preRemainderLength);

                    var parsedPreRemainderParameters = preRemainderArgs.Zip(preRemainderParameters, (s, info) => info.ConvertToParameterType(s)).ToList();

                    string[] remainderArgs = args.Skip(preRemainderLength).ToArray();
                    object parsedRemainder = null;
                    if (lastPar.IsRemainder)
                        parsedRemainder = lastPar.ConvertToParameterType(string.Join(" ", remainderArgs));
                    if (lastPar.IsParamsArray)
                        parsedRemainder = lastPar.ConvertToParameterTypeArray(remainderArgs);

                    parsedPreRemainderParameters.Add(parsedRemainder);
                    return parsedPreRemainderParameters.ToArray();
                }
                else return args.Zip(Parameters, (s, info) => info.ConvertToParameterType(s)).ToArray();
            }
        }

        /// <summary>
        /// Invokes the command using the arguments from the given command message and returns the execution result.
        /// </summary>
        public IExecutionResult Execute(IMessage msg, int argPos)
        {
            ArgumentHelper.ExtractNameAndArgs(msg, argPos, out string name, out string[] args);

            object[] parameters;
            try
            {
                parameters = ParseArgumentsToParameters(args);
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

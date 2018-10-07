using System;
using System.Reflection;
using System.ComponentModel;

namespace CMDotNet
{
    /// <summary>
    /// Holds information about a command parameter.
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Is the parameter a remainder?
        /// </summary>
        public bool IsRemainder { get; set; }
        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// The parameter's default value.
        /// </summary>
        public object DefaultValue { get; set; }

        private ParameterInfo() { }
        /// <summary>
        /// Builds a <see cref="ParameterInfo"/> from a reflected <see cref="System.Reflection.ParameterInfo"/>.
        /// </summary>
        public static ParameterInfo FromParameter(System.Reflection.ParameterInfo parameter)
        {
            return new ParameterInfo
            {
                Name = parameter.Name,
                IsRemainder = parameter.GetCustomAttribute<RemainderAttribute>() != null,
                Type = parameter.ParameterType,
                DefaultValue = parameter.DefaultValue
            };
        }
        /// <summary>
        /// Converts the specified string into the parameter type, if possible. Throws a <see cref="NotSupportedException"/> if the type can't be converted.
        /// </summary>
        public object ConvertToParameterType(string s)
            => TypeDescriptor.GetConverter(Type).ConvertFrom(s);
    }
}

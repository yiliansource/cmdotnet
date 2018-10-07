using System;
using System.Linq;
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
        public bool IsRemainder { get; private set; }
        /// <summary>
        /// Is the parameter type a params array?
        /// </summary>
        public bool IsParamsArray { get; private set; }
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
                IsParamsArray = parameter.ParameterType.IsArray && parameter.GetCustomAttribute<ParamArrayAttribute>() != null,
                Type = parameter.ParameterType,
                DefaultValue = parameter.DefaultValue
            };
        }

        /// <summary>
        /// Converts the specified string into the parameter type, if possible. Throws a <see cref="NotSupportedException"/> if the type can't be converted.
        /// </summary>
        public object ConvertToParameterType(string arg)
            => TypeDescriptor.GetConverter(Type).ConvertFrom(arg);

        /// <summary>
        /// Converts the specfied arguments into an array of the element type of the parameter. Throws a <see cref="NotSupportedException"/> if the type can't be converted.
        /// </summary>
        public Array ConvertToParameterTypeArray(string[] args)
        {
            Type arrayType = Type.GetElementType();
            TypeConverter descriptor = TypeDescriptor.GetConverter(arrayType);
            object[] objectArray = args.Select(arg => descriptor.ConvertFrom(arg)).ToArray();

            Array destinationArray = Array.CreateInstance(arrayType, objectArray.Length);
            Array.Copy(objectArray, destinationArray, objectArray.Length);
            return destinationArray;
        }
    }
}

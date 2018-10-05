using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace CMDotNet
{
    public class ParameterInfo
    {
        public string Name { get; set; }
        public bool IsRemainder { get; set; }
        public Type Type { get; set; }
        public object DefaultValue { get; set; }

        private ParameterInfo() { }
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

        public object ConvertToParameterType(string s)
            => TypeDescriptor.GetConverter(Type).ConvertFrom(s);
    }
}

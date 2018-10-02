using System;
using System.Reflection;

namespace YSource
{
    public class Command : IEquatable<Command>
    {
        public string Name { get; set; }
        public string[] Aliases { get; set; }
        public Attribute[] Attributes { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public MethodInfo Method { get; set; }

        public bool Equals(Command other)
            => Equals(Method.MethodHandle.Value.ToInt32(), other.Method.MethodHandle.Value.ToInt32());

        public override string ToString()
            => $"{ Name } (Command)";
    }
}

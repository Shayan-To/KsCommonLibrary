#nullable enable

using System;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class PropertyAttributeAttribute : Attribute
    {
        public PropertyAttributeAttribute(Type attributeType, params object?[] arguments)
        {
            this.AttributeType = attributeType;
            this.Arguments = arguments;
        }

        public Type AttributeType { get; }
        public object?[] Arguments { get; }
        public string[]? Names { get; set; }
        public object?[]? Values { get; set; }

    }

}

#nullable enable

using System;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class CollectedCalledAttribute : Attribute
    {

        public CollectedCalledAttribute(string methodName)
        {
            this.MethodName = methodName;
        }

        public string MethodName { get; }

    }
}

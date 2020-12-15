#nullable enable

using System;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ReactiveOutputPropertyAttribute : Attribute
    {

        public string? PropertyName { get; set; } = null;
        public string? HelperName { get; set; } = null;
        public string? DefaultValueTag { get; set; } = null;
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;
        public AccessModifier HelperAccessModifier { get; set; } = AccessModifier.Private;
        public bool Virtual { get; set; } = false;
        public bool New { get; set; } = false;

    }
}

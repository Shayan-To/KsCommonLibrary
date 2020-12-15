#nullable enable

using System;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ReactivePropertyAttribute : Attribute
    {

        public string? PropertyName { get; set; } = null;
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;
        public AccessModifier GetterAccessModifier { get; set; } = default;
        public AccessModifier SetterAccessModifier { get; set; } = default;
        public bool Virtual { get; set; } = false;
        public bool New { get; set; } = false;

    }
}

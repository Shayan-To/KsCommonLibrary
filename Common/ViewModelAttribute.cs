#nullable enable

using System;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ViewModelAttribute : Attribute
    {

        public Type? View { get; set; }

    }

}

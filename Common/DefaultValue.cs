#nullable enable

using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public static class DefaultValue
    {

        public static T? Get<T>(string? tag = null)
        {
            for (var i = 0; i < Generators.Count; i++)
            {
                var g = Generators[i];
                var gg = g.Invoke(typeof(T), tag);
                if (gg.Item1)
                {
                    return (T?) gg.Item2;
                }
            }
            return default;
        }

        public static List<Func<Type, string?, (bool, object?)>> Generators { get; } = new();
    }
}

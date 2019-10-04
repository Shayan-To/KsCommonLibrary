﻿using System;

namespace Ks
{
    namespace Common
    {
        public static class DefaultCacher<T> where T : new()
        {

            public static void SetValue(T Value)
            {
                _Value.Value = Value;
            }

            private static readonly System.Threading.ThreadLocal<T> _Value = new System.Threading.ThreadLocal<T>(() => new T());

            public static T Value
            {
                get
                {
                    return _Value.Value;
                }
            }
        }
    }
}
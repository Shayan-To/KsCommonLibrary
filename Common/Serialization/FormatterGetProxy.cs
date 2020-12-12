﻿namespace Ks.Common
{
    public class FormatterGetProxy
    {
        public FormatterGetProxy(Formatter Formatter)
        {
            this._Formatter = Formatter;
        }

        protected internal T Get<T>(string Name)
        {
            return this.Formatter.Get<T>(Name);
        }

        protected internal object Get(string Name)
        {
            return this.Formatter.Get(Name);
        }

        protected internal void Get<T>(string Name, T Obj)
        {
            this.Formatter.Get(Name, Obj);
        }

        protected internal void Get(string Name, object Obj)
        {
            this.Formatter.Get(Name, Obj);
        }

        private readonly Formatter _Formatter;

        public Formatter Formatter
        {
            get
            {
                return this._Formatter;
            }
        }
    }
}

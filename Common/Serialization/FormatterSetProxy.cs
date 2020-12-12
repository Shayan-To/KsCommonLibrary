namespace Ks.Common
{
    public class FormatterSetProxy
    {
        public FormatterSetProxy(Formatter Formatter)
        {
            this._Formatter = Formatter;
        }

        protected internal void Set<T>(string Name, T Obj)
        {
            this.Formatter.Set(Name, Obj);
        }

        protected internal void Set(string Name, object Obj)
        {
            this.Formatter.Set(Name, Obj);
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

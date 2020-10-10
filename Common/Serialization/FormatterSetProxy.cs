namespace Ks.Common
{
    public class FormatterSetProxy
    {
        public FormatterSetProxy(Formatter Formatter)
        {
            this.Formatter = Formatter;
        }

        protected internal void Set<T>(string Name, T Obj)
        {
            this.Formatter.Set(Name, Obj);
        }

        protected internal void Set(string Name, object Obj)
        {
            this.Formatter.Set(Name, Obj);
        }

        public Formatter Formatter { get; }
    }
}

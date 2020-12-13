using System;
using System.Windows.Markup;

namespace Ks
{
    namespace Common
    {
        public class AddChildList<T> : NotifyingList<T>, IAddChild
        {
            public void AddChild(object value)
            {
                this.Add((T)value);
            }

            public void AddText(string text)
            {
                throw new NotSupportedException();
            }
        }
    }
}

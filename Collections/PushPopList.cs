using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public interface IPushPop<T>
        {
            void Push(T Item);
            T Pop();
            T Peek();
            bool CanPop();
        }

        public class PushPopList<T> : List<T>, IPushPop<T>
        {
            public void Push(T Item)
            {
                this.Add(Item);
            }

            public T Pop()
            {
                var I = this.Count - 1;
                var R = this[I];
                this.RemoveAt(I);
                return R;
            }

            public T Peek()
            {
                return this[this.Count - 1];
            }

            public bool CanPop()
            {
                return this.Count != 0;
            }
        }
    }
}

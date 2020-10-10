using System.Collections.Generic;

namespace Ks.Common
{
    public abstract class ObjectTraverseProvider
    {
        protected abstract IEnumerable<Void> GetSetPropertiesOverride(PropertyTraverseHandler Handler);
    }


    public class PropertyTraverseHandler
    {
    }

    public struct PropertyTraverseCurrent
    {
        public PropertyTraverseCurrent(PropertyTraverseHandler Handler)
        {
            this.Handler = Handler;
        }

        public PropertyTraverseHandler Handler { get; }
    }

    public interface PropertyTraverseProcessor
    {
        void Process<T>(PropertyTraverseHandler Prop);
    }
}

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
            this._Handler = Handler;
        }

        private readonly PropertyTraverseHandler _Handler;

        public PropertyTraverseHandler Handler
        {
            get
            {
                return this._Handler;
            }
        }
    }

    public interface PropertyTraverseProcessor
    {
        void Process<T>(PropertyTraverseHandler Prop);
    }
}

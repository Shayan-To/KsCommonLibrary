namespace Ks
{
    namespace Ks.Common.MVVM
    {
        public class WorldedObject<TWorld> : BindableBase
        {
            public WorldedObject(TWorld World)
            {
                this._World = World;
            }

            private readonly TWorld _World;

            public TWorld World
            {
                get
                {
                    return this._World;
                }
            }
        }
    }
}

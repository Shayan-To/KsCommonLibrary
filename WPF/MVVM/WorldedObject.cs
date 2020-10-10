namespace Ks.Common.MVVM
{
    public class WorldedObject<TWorld> : BindableBase
    {
        public WorldedObject(TWorld World)
        {
            this.World = World;
        }

        public TWorld World { get; }
    }
}

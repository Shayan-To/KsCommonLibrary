namespace Ks.Common.MVVM
{
    public struct NavigationData
    {
        public NavigationData(NavigationFrame Frame, bool AddToStack = true, bool ForceToStack = false)
        {
            this.Frame = Frame;
            this.AddToStack = AddToStack;
            this.ForceToStack = ForceToStack;
        }

        public NavigationFrame Frame { get; }

        public bool AddToStack { get; }

        public bool ForceToStack { get; }
    }
}

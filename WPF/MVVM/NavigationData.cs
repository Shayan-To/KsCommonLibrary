namespace Ks.Common.MVVM
{
    public struct NavigationData
    {
        public NavigationData(NavigationFrame Frame, bool AddToStack = true, bool ForceToStack = false)
        {
            this._Frame = Frame;
            this._AddToStack = AddToStack;
            this._ForceToStack = ForceToStack;
        }

        private readonly NavigationFrame _Frame;

        public NavigationFrame Frame
        {
            get
            {
                return this._Frame;
            }
        }

        private readonly bool _AddToStack;

        public bool AddToStack
        {
            get
            {
                return this._AddToStack;
            }
        }

        private readonly bool _ForceToStack;

        public bool ForceToStack
        {
            get
            {
                return this._ForceToStack;
            }
        }
    }
}

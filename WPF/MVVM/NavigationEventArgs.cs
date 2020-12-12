using System;

namespace Ks.Common.MVVM
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(NavigationType NavigationType)
        {
            this._NavigationType = NavigationType;
        }

        private readonly NavigationType _NavigationType;

        public NavigationType NavigationType
        {
            get
            {
                return this._NavigationType;
            }
        }
    }
}

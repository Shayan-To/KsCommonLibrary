using System;

namespace Ks.Common.MVVM
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(NavigationType NavigationType)
        {
            this.NavigationType = NavigationType;
        }

        public NavigationType NavigationType { get; }
    }
}

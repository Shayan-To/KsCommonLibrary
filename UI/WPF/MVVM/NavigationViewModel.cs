using System;

namespace Ks.Common.MVVM
{
    public abstract class NavigationViewModel : ViewModel
    {
        public NavigationViewModel(KsApplication KsApplication) : base(KsApplication)
        {
            if (!typeof(INavigationView).IsAssignableFrom(this.Metadata.ViewType))
            {
                throw new InvalidOperationException("Navigation views must implement INavigationView.");
            }
        }

        public NavigationViewModel() : base()
        {
        }

        public void NavigateTo(ViewModel ViewModel, bool AddToStack = true, bool ForceToStack = false)
        {
            this.KsApplicationBase.NavigateTo(this, ViewModel, AddToStack, ForceToStack);
        }
    }
}

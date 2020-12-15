using System.Windows;

using Ks.Common.Controls;

namespace Ks.Common.MVVM
{
    [ViewModelMetadata(typeof(Controls.Window), IsSingleInstance = true)]
    public class WindowViewModel : NavigationViewModel
    {
        public WindowViewModel(KsApplication KsApplication) : base(KsApplication)
        {
        }

        public WindowViewModel() : base()
        {
        }
    }
}

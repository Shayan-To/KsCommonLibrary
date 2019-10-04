using System.Windows.Controls;
using Ks.Common.Controls;

namespace Ks
{
    namespace Ks.Common.MVVM
    {
        internal interface INavigationView
        {
            Page Content { get; set; }
        }
    }
}

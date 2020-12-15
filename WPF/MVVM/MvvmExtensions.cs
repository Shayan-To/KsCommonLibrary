using System.Windows.Controls;

using Ks.Common.Controls;

using Page = Ks.Common.Controls.Page;

namespace Ks.Common.MVVM
{
    public static class MvvmExtensions
    {
        public static bool IsNavigation(this ViewModel Self)
        {
            return Self is NavigationViewModel;
        }

        internal static INavigationView GetNavigationView(this ViewModel Self)
        {
            return (INavigationView) Self.View;
        }

        internal static void SetView(this ViewModel Navigation, ViewModel ViewModel)
        {
            Navigation.GetNavigationView().SetContent((Page) ViewModel?.View);
        }

        internal static void SetContent(this INavigationView NavigationView, Page View)
        {
            var Prev = NavigationView.Content;
            if (Prev == View)
            {
                return;
            }

            if (Prev != null)
            {
                Prev.ParentView = null;
            }

            if (View != null)
            {
                if (View.ParentView != null)
                {
                    View.ParentView.Content = null;
                }

                View.ParentView = NavigationView;
            }

            NavigationView.Content = View;
        }
    }
}

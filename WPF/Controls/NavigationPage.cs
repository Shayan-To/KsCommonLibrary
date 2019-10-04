using System.Windows;

namespace Ks
{
    namespace Ks.Common.Controls
    {
        [System.ComponentModel.DesignTimeVisible(false)]
        public class NavigationPage : Page, INavigationView
        {
            static NavigationPage()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationPage), new FrameworkPropertyMetadata(typeof(NavigationPage)));
            }

            private Page INavigationView_Content
            {
                get
                {
                    return (Page)this.Content;
                }
                set
                {
                    this.Content = value;
                }
            }
        }
    }
}

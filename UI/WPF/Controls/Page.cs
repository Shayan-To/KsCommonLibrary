using System.Windows;
using System.Windows.Controls;

namespace Ks.Common.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public class Page : ContentControl
    {
        static Page()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(Page)));
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Page), new PropertyMetadata(null));

        public string Title
        {
            get => (string) this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }

        internal MVVM.INavigationView ParentView { get; set; }
    }
}

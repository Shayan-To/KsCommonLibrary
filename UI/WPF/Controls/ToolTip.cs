using System.Threading.Tasks;
using System.Windows;

namespace Ks.Common.Controls
{
    public class ToolTip : System.Windows.Controls.ToolTip
    {

        // ToDo Make the popup be above the mouse using the CustomPopupPlacementCallback and Placement properties.

        static ToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
            TextBlock.KsLanguageProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(KsLanguage_Changed));
        }

        private static void KsLanguage_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ToolTip) D;
            Self.Update();
        }

        private async void Update()
        {
            var Text = this.Text;
            if (Text == null)
            {
                return;
            }

            var Lang = TextBlock.GetKsLanguage(this);
            var OText = Lang?.GetTranslation(Text) ?? Text;

            this.Visibility = (OText.Length != 0) ? Visibility.Visible : Visibility.Collapsed;
            this.Content = OText;

            // There is a bug that the content changes, but the UI does not show the change. Let the translation for a language be empty and non-empty for another. Switching to the non-empty one will show an empty tool tip.
            // ToDo This is a very bad workaround. Correct it.
            await Task.Delay(1);
            this.Content = "";
            await Task.Delay(1);
            this.Content = OText;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ToolTip), new PropertyMetadata(null, Text_Changed, Text_Coerce));

        private static object Text_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, ToolTip)

            // Dim Value = DirectCast(BaseValue, String)

            return BaseValue;
        }

        private static void Text_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ToolTip) D;

            // Dim OldValue = DirectCast(E.OldValue, String)
            // Dim NewValue = DirectCast(E.NewValue, String)

            Self.Update();
        }

        public string Text
        {
            get => (string) this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }
    }
}

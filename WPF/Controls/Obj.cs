using System.Windows;

namespace Ks
{
    namespace Common.Controls
    {
        public class Obj : FrameworkContentElement // ToDo For the binding on the Obj to work, this was forced to change to a FrameworkElement or a FrameworkContentElement. Otherwise, the AddLogicalChild on the TextBlock has no effect and DataContext, TemplatedParent, ElementName, FindAncestor, etc. will not work in bindings on the Obj in the Obj list. Find out why this does not work, and remove this unnecessary base class. (why bindings work on Brush, RotateTransform, etc.?)
        {
            internal void ReportParent(TextBlock Parent)
            {
                this._TextBlock = Parent;
            }

            public static readonly DependencyProperty ObjtProperty = DependencyProperty.Register("Objt", typeof(object), typeof(Obj), new PropertyMetadata(null, Objt_Changed, Objt_Coerce));

            private static object Objt_Coerce(DependencyObject D, object BaseValue)
            {
                var Self = (Obj)D;

                // Dim Value = DirectCast(BaseValue, Object)

                return BaseValue;
            }

            private static void Objt_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
            {
                var Self = (Obj)D;

                // Dim OldValue = DirectCast(E.OldValue, Object)
                // Dim NewValue = DirectCast(E.NewValue, Object)

                Self.TextBlock?.ReportObjChanged();
            }

            public object Objt
            {
                get
                {
                    return (object)this.GetValue(ObjtProperty);
                }
                set
                {
                    this.SetValue(ObjtProperty, value);
                }
            }

            private TextBlock _TextBlock;

            public TextBlock TextBlock
            {
                get
                {
                    return this._TextBlock;
                }
            }
        }
    }
}

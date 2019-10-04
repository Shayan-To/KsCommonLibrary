using System.Windows;

namespace Ks
{
    namespace Ks.Common.Controls
    {
        public class Obj : FrameworkContentElement // ToDo For the binding on the Obj to work, this was forced to change to a FrameworkElement or a FrameworkContentElement. Otherwise, the AddLogicalChild on the TextBlock has no effect and DataContext, TemplatedParent, ElementName, FindAncestor, etc. will not work in bindings on the Obj in the Obj list. Find out why this does not work, and remove this unnecessary base class. (why bindings work on Brush, RotateTransform, etc.?)
        {
            internal void ReportParent(TextBlock Parent)
            {
                this._TextBlock = Parent;
            }

            public static readonly DependencyProperty ObjProperty = DependencyProperty.Register("Obj", typeof(object), typeof(Obj), new PropertyMetadata(null, Obj_Changed, Obj_Coerce));

            private static object Obj_Coerce(DependencyObject D, object BaseValue)
            {
                var Self = (Obj)D;

                // Dim Value = DirectCast(BaseValue, Object)

                return BaseValue;
            }

            private static void Obj_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
            {
                var Self = (Obj)D;

                // Dim OldValue = DirectCast(E.OldValue, Object)
                // Dim NewValue = DirectCast(E.NewValue, Object)

                Self.TextBlock?.ReportObjChanged();
            }

            public object Obj
            {
                get
                {
                    return (object)this.GetValue(ObjProperty);
                }
                set
                {
                    this.SetValue(ObjProperty, value);
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

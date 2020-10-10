using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using System.Windows.Documents;

namespace Ks.Common.MVVM
{
    public class Utils : DependencyObject
    {

        // #Region "ExtraLogicalChildren Property"
        // Public Shared ReadOnly ExtraLogicalChildrenProperty As DependencyProperty = DependencyProperty.RegisterAttached("ExtraLogicalChildren", GetType(AddChildList(Of Object)), GetType(Utils), New PropertyMetadata(Nothing, AddressOf ExtraLogicalChildren_Changed))

        // Private Shared Sub ExtraLogicalChildren_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        // Dim Self = DirectCast(D, Utils)

        // Dim OldValue = DirectCast(E.OldValue, AddChildList(Of Object))
        // Dim NewValue = DirectCast(E.NewValue, AddChildList(Of Object))

        // Dim Handler = Sub(S2 As Object, E2 As EventArgs)

        // End Sub

        // End Sub

        // Public Shared Function GetExtraLogicalChildren(ByVal Element As DependencyObject) As AddChildList(Of Object)
        // If Element Is Nothing Then
        // Throw New ArgumentNullException("Element")
        // End If

        // Return DirectCast(Element.GetValue(ExtraLogicalChildrenProperty), AddChildList(Of Object))
        // End Function

        // Public Shared Sub SetExtraLogicalChildren(ByVal Element As DependencyObject, ByVal Value As AddChildList(Of Object))
        // If Element Is Nothing Then
        // Throw New ArgumentNullException("Element")
        // End If

        // Element.SetValue(ExtraLogicalChildrenProperty, Value)
        // End Sub
        // #End Region

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached("Document", typeof(FlowDocument), typeof(Utils), new PropertyMetadata(null, Document_Changed, Document_Coerce));

        private static object Document_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as RichTextBox;
            if (Self == null)
            {
                return DocumentProperty.DefaultMetadata.DefaultValue;
            }

            var Value = (FlowDocument) BaseValue;

            return BaseValue;
        }

        private static void Document_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (RichTextBox) D;

            var OldValue = (FlowDocument) E.OldValue;
            var NewValue = (FlowDocument) E.NewValue;

            Self.Document = NewValue;
        }

        public static FlowDocument GetDocument(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (FlowDocument) Element.GetValue(DocumentProperty);
        }

        public static void SetDocument(DependencyObject Element, FlowDocument Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(DocumentProperty, Value);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached("ViewModel", typeof(ViewModel), typeof(Utils), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, ViewModel_Changed, ViewModel_Coerce));

        private static object ViewModel_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = TryCast(D, DependencyObject)
            // If Self Is Nothing Then
            // Return ViewModelProperty.DefaultMetadata.DefaultValue
            // End If

            // Dim Value = DirectCast(BaseValue, ViewModel)

            return BaseValue;
        }

        private static void ViewModel_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
        }

        public static ViewModel GetViewModel(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (ViewModel) Element.GetValue(ViewModelProperty);
        }

        public static void SetViewModel(DependencyObject Element, ViewModel Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(ViewModelProperty, Value);
        }

        public static readonly DependencyProperty DesignViewModelTypeProperty = DependencyProperty.RegisterAttached("DesignViewModelType", typeof(Type), typeof(Utils), new PropertyMetadata(null, DesignViewModelType_Changed, DesignViewModelType_Coerce), DesignViewModelType_Validate);

        private static bool DesignViewModelType_Validate(object BaseValue)
        {
            var Value = (Type) BaseValue;

            return Value == null || typeof(ViewModel).IsAssignableFrom(Value);
        }

        private static object DesignViewModelType_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as FrameworkElement;
            if (Self == null)
            {
                return DesignViewModelTypeProperty.DefaultMetadata.DefaultValue;
            }

            // Dim Value = DirectCast(BaseValue, Type)

            return BaseValue;
        }

        private static void DesignViewModelType_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            if (!KsApplication.IsInDesignMode)
            {
                return;
            }

            var Self = (FrameworkElement) D;

            // Dim OldValue = DirectCast(E.OldValue, Type)
            var NewValue = (Type) E.NewValue;

            var ViewModel = (ViewModel) NewValue.CreateInstance();

            Self.DataContext = ViewModel;
            SetViewModel(Self, ViewModel);
            ViewModel.View = (Control) Self;
        }

        public static Type GetDesignViewModelType(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (Type) Element.GetValue(DesignViewModelTypeProperty);
        }

        public static void SetDesignViewModelType(DependencyObject Element, Type Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(DesignViewModelTypeProperty, Value);
        }

        public static readonly DependencyProperty IndexInParentProperty = DependencyProperty.RegisterAttached("IndexInParent", typeof(int), typeof(Utils), new PropertyMetadata(-1, IndexInParent_Changed, IndexInParent_Coerce));

        private static object IndexInParent_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as DependencyObject;
            // If Self Is Nothing Then
            // Return IndexInParentProperty.DefaultMetadata.DefaultValue
            // End If

            var Parent = LogicalTreeHelper.GetParent(Self);
            if (!CheckedParents.Add(Parent))
            {
                return BaseValue;
            }

            var Res = -1;
            var I = 0;
            foreach (DependencyObject C in LogicalTreeHelper.GetChildren(Parent))
            {
                if (Self == C)
                {
                    Assert.True(Res == -1);
                    Res = I;
                }
                else
                {
                    SetIndexInParent(C, I);
                }

                I += 1;
            }

            Assert.False(Res == -1);

            // Assert.True(InCheckParents.Remove(Parent))

            // Dim Value = DirectCast(BaseValue, Integer)

            return Res;
        }

        private static void IndexInParent_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
        }

        public static int GetIndexInParent(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (int) Element.GetValue(IndexInParentProperty);
        }

        public static void SetIndexInParent(DependencyObject Element, int Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(IndexInParentProperty, Value);
        }

        // Private Shared ReadOnly InCheckParents As HashSet(Of DependencyObject) = New HashSet(Of DependencyObject)()
        private static readonly HashSet<DependencyObject> CheckedParents = new HashSet<DependencyObject>();

        public static readonly DependencyProperty UpTPProp1Property = DependencyProperty.RegisterAttached("UpTPProp1", typeof(object), typeof(Utils), new PropertyMetadata(null, UpTPProp1_Changed));

        private static void UpTPProp1_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (FrameworkElement) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            var NewValue = (object) E.NewValue;

            if (Self.TemplatedParent != null)
            {
                SetUpTPProp1(Self.TemplatedParent, NewValue);
            }
        }

        public static object GetUpTPProp1(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(UpTPProp1Property);
        }

        public static void SetUpTPProp1(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(UpTPProp1Property, Value);
        }

        public static readonly DependencyProperty UpTPProp2Property = DependencyProperty.RegisterAttached("UpTPProp2", typeof(object), typeof(Utils), new PropertyMetadata(null, UpTPProp2_Changed));

        private static void UpTPProp2_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (FrameworkElement) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            var NewValue = (object) E.NewValue;

            if (Self.TemplatedParent != null)
            {
                SetUpTPProp2(Self.TemplatedParent, NewValue);
            }
        }

        public static object GetUpTPProp2(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(UpTPProp2Property);
        }

        public static void SetUpTPProp2(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(UpTPProp2Property, Value);
        }

        public static readonly DependencyProperty UpTPProp3Property = DependencyProperty.RegisterAttached("UpTPProp3", typeof(object), typeof(Utils), new PropertyMetadata(null, UpTPProp3_Changed));

        private static void UpTPProp3_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (FrameworkElement) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            var NewValue = (object) E.NewValue;

            if (Self.TemplatedParent != null)
            {
                SetUpTPProp3(Self.TemplatedParent, NewValue);
            }
        }

        public static object GetUpTPProp3(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(UpTPProp3Property);
        }

        public static void SetUpTPProp3(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(UpTPProp3Property, Value);
        }

        public static readonly DependencyProperty Prop1Property = DependencyProperty.RegisterAttached("Prop1", typeof(object), typeof(Utils), new PropertyMetadata(null));

        public static object GetProp1(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(Prop1Property);
        }

        public static void SetProp1(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(Prop1Property, Value);
        }

        public static readonly DependencyProperty Prop2Property = DependencyProperty.RegisterAttached("Prop2", typeof(object), typeof(Utils), new PropertyMetadata(null));

        public static object GetProp2(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(Prop2Property);
        }

        public static void SetProp2(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(Prop2Property, Value);
        }

        public static readonly DependencyProperty Prop3Property = DependencyProperty.RegisterAttached("Prop3", typeof(object), typeof(Utils), new PropertyMetadata(null));

        public static object GetProp3(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(Prop3Property);
        }

        public static void SetProp3(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(Prop3Property, Value);
        }

        public static readonly DependencyProperty Prop4Property = DependencyProperty.RegisterAttached("Prop4", typeof(object), typeof(Utils), new PropertyMetadata(null));

        public static object GetProp4(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(Prop4Property);
        }

        public static void SetProp4(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(Prop4Property, Value);
        }

        public static readonly DependencyProperty Prop5Property = DependencyProperty.RegisterAttached("Prop5", typeof(object), typeof(Utils), new PropertyMetadata(null));

        public static object GetProp5(DependencyObject Element)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            return (object) Element.GetValue(Prop5Property);
        }

        public static void SetProp5(DependencyObject Element, object Value)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }

            Element.SetValue(Prop5Property, Value);
        }

        public static readonly DependencyProperty SnapsToDevicePixelsProperty = DependencyProperty.RegisterAttached("SnapsToDevicePixels", typeof(bool), typeof(Utils), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, SnapsToDevicePixels_Changed, SnapsToDevicePixels_Coerce));

        private static object SnapsToDevicePixels_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = TryCast(D, UIElement)
            // If Self Is Nothing Then
            // Return SnapsToDevicePixelsProperty.DefaultMetadata.DefaultValue
            // End If

            // Dim Value = DirectCast(BaseValue, Boolean)

            return BaseValue;
        }

        private static void SnapsToDevicePixels_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = D as UIElement;

            if (Self == null)
            {
                return;
            }

            // Dim OldValue = DirectCast(E.OldValue, Boolean)
            var NewValue = (bool) E.NewValue;

            Self.SnapsToDevicePixels = NewValue;
        }

        public static bool GetSnapsToDevicePixels(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (bool) Element.GetValue(SnapsToDevicePixelsProperty);
        }

        public static void SetSnapsToDevicePixels(DependencyObject Element, bool Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(SnapsToDevicePixelsProperty, Value);
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(System.Windows.Media.Brush), typeof(Utils), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, Foreground_Changed, Foreground_Coerce));

        private static object Foreground_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = TryCast(D, UIElement)
            // If Self Is Nothing Then
            // Return ForegroundProperty.DefaultMetadata.DefaultValue
            // End If

            // Dim Value = DirectCast(BaseValue, Media.Brush)

            return BaseValue;
        }

        private static void Foreground_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = D as UIElement;

            if (Self == null)
            {
                return;
            }

            // Dim OldValue = DirectCast(E.OldValue, Media.Brush)
            var NewValue = (System.Windows.Media.Brush) E.NewValue;

            System.Windows.Documents.TextElement.SetForeground(Self, NewValue);

            Self = (Self as ContentPresenter)?.Content as UIElement;
            if (Self == null)
            {
                return;
            }

            System.Windows.Documents.TextElement.SetForeground(Self, NewValue);
        }

        public static System.Windows.Media.Brush GetForeground(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (System.Windows.Media.Brush) Element.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject Element, System.Windows.Media.Brush Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(ForegroundProperty, Value);
        }

        public static readonly DependencyProperty DescriptionToolTipProperty = DependencyProperty.RegisterAttached("DescriptionToolTip", typeof(string), typeof(Utils), new PropertyMetadata(null, DescriptionToolTip_Changed, DescriptionToolTip_Coerce));

        private static object DescriptionToolTip_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as FrameworkElement;
            if (Self == null)
            {
                return DescriptionToolTipProperty.DefaultMetadata.DefaultValue;
            }

            var Value = (string) BaseValue;

            return BaseValue;
        }

        private static void DescriptionToolTip_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (FrameworkElement) D;

            var OldValue = (string) E.OldValue;
            var NewValue = (string) E.NewValue;

            Self.ToolTip = new Controls.ToolTip() { Text = "Desc@" + NewValue ?? "" };
        }

        public static string GetDescriptionToolTip(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (string) Element.GetValue(DescriptionToolTipProperty);
        }

        public static void SetDescriptionToolTip(DependencyObject Element, string Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(DescriptionToolTipProperty, Value);
        }

        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip", typeof(string), typeof(Utils), new PropertyMetadata(null, ToolTip_Changed, ToolTip_Coerce));

        private static object ToolTip_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as FrameworkElement;
            if (Self == null)
            {
                return ToolTipProperty.DefaultMetadata.DefaultValue;
            }

            var Value = (string) BaseValue;

            return BaseValue;
        }

        private static void ToolTip_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (FrameworkElement) D;

            var OldValue = (string) E.OldValue;
            var NewValue = (string) E.NewValue;

            Self.ToolTip = new Controls.ToolTip() { Text = NewValue ?? "" };
        }

        public static string GetToolTip(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (string) Element.GetValue(ToolTipProperty);
        }

        public static void SetToolTip(DependencyObject Element, string Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(ToolTipProperty, Value);
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(Utils), new PropertyMetadata("", Password_Changed, Password_Coerce));
        private static readonly HashSet<PasswordBox> PasswordProperty_Controls = new HashSet<PasswordBox>();

        private static object Password_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = D as PasswordBox;
            if (Self == null)
            {
                return PasswordProperty.DefaultMetadata.DefaultValue;
            }

            var Value = (string) BaseValue;

            return BaseValue;
        }

        private static void Password_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PasswordBox) D;

            var OldValue = (string) E.OldValue;
            var NewValue = (string) E.NewValue;

            if (PasswordProperty_Controls.Add(Self))
            {
                Self.PasswordChanged += (S2, E2) =>
                {
                    var PB = (PasswordBox) S2;
                    SetPassword(PB, PB.Password);
                };
            }

            Self.Password = NewValue;
        }

        public static string GetPassword(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (string) Element.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject Element, string Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(PasswordProperty, Value);
        }

        public const bool True = true;
        public const bool False = false;
    }
}

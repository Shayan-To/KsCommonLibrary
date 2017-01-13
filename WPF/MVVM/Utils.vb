Imports System.Windows.Controls.Primitives
Imports System.Windows.Documents

Namespace Common.MVVM

    Public Class Utils
        Inherits DependencyObject

        '#Region "ExtraLogicalChildren Property"
        '        Public Shared ReadOnly ExtraLogicalChildrenProperty As DependencyProperty = DependencyProperty.RegisterAttached("ExtraLogicalChildren", GetType(AddChildList(Of Object)), GetType(Utils), New PropertyMetadata(Nothing, AddressOf ExtraLogicalChildren_Changed))

        '        Private Shared Sub ExtraLogicalChildren_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        '            Dim Self = DirectCast(D, Utils)

        '            Dim OldValue = DirectCast(E.OldValue, AddChildList(Of Object))
        '            Dim NewValue = DirectCast(E.NewValue, AddChildList(Of Object))

        '            Dim Handler = Sub(S2 As Object, E2 As EventArgs)

        '                          End Sub

        '        End Sub

        '        Public Shared Function GetExtraLogicalChildren(ByVal Element As DependencyObject) As AddChildList(Of Object)
        '            If Element Is Nothing Then
        '                Throw New ArgumentNullException("Element")
        '            End If

        '            Return DirectCast(Element.GetValue(ExtraLogicalChildrenProperty), AddChildList(Of Object))
        '        End Function

        '        Public Shared Sub SetExtraLogicalChildren(ByVal Element As DependencyObject, ByVal Value As AddChildList(Of Object))
        '            If Element Is Nothing Then
        '                Throw New ArgumentNullException("Element")
        '            End If

        '            Element.SetValue(ExtraLogicalChildrenProperty, Value)
        '        End Sub
        '#End Region

#Region "Document Property"
        Public Shared ReadOnly DocumentProperty As DependencyProperty = DependencyProperty.RegisterAttached("Document", GetType(FlowDocument), GetType(Utils), New PropertyMetadata(Nothing, AddressOf Document_Changed, AddressOf Document_Coerce))

        Private Shared Function Document_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = TryCast(D, RichTextBox)
            If Self Is Nothing Then
                Return DocumentProperty.DefaultMetadata.DefaultValue
            End If

            Dim Value = DirectCast(BaseValue, FlowDocument)

            Return BaseValue
        End Function

        Private Shared Sub Document_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, RichTextBox)

            Dim OldValue = DirectCast(E.OldValue, FlowDocument)
            Dim NewValue = DirectCast(E.NewValue, FlowDocument)

            Self.Document = NewValue
        End Sub

        Public Shared Function GetDocument(ByVal Element As DependencyObject) As FlowDocument
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(DocumentProperty), FlowDocument)
        End Function

        Public Shared Sub SetDocument(ByVal Element As DependencyObject, ByVal Value As FlowDocument)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(DocumentProperty, Value)
        End Sub
#End Region

#Region "ViewModel Property"
        Public Shared ReadOnly ViewModelProperty As DependencyProperty = DependencyProperty.RegisterAttached("ViewModel", GetType(ViewModel), GetType(Utils), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.Inherits, AddressOf ViewModel_Changed, AddressOf ViewModel_Coerce))

        Private Shared Function ViewModel_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = TryCast(D, DependencyObject)
            'If Self Is Nothing Then
            '    Return ViewModelProperty.DefaultMetadata.DefaultValue
            'End If

            'Dim Value = DirectCast(BaseValue, ViewModel)

            Return BaseValue
        End Function

        Private Shared Sub ViewModel_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            'Dim Self = DirectCast(D, DependencyObject)

            'Dim OldValue = DirectCast(E.OldValue, ViewModel)
            'Dim NewValue = DirectCast(E.NewValue, ViewModel)
        End Sub

        Public Shared Function GetViewModel(ByVal Element As DependencyObject) As ViewModel
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(ViewModelProperty), ViewModel)
        End Function

        Public Shared Sub SetViewModel(ByVal Element As DependencyObject, ByVal Value As ViewModel)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(ViewModelProperty, Value)
        End Sub
#End Region

#Region "DesignViewModelType Property"
        Public Shared ReadOnly DesignViewModelTypeProperty As DependencyProperty = DependencyProperty.RegisterAttached("DesignViewModelType", GetType(Type), GetType(Utils), New PropertyMetadata(Nothing, AddressOf DesignViewModelType_Changed, AddressOf DesignViewModelType_Coerce), AddressOf DesignViewModelType_Validate)

        Private Shared Function DesignViewModelType_Validate(ByVal BaseValue As Object) As Boolean
            Dim Value = DirectCast(BaseValue, Type)

            Return Value Is Nothing OrElse GetType(ViewModel).IsAssignableFrom(Value)
        End Function

        Private Shared Function DesignViewModelType_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = TryCast(D, FrameworkElement)
            If Self Is Nothing Then
                Return DesignViewModelTypeProperty.DefaultMetadata.DefaultValue
            End If

            'Dim Value = DirectCast(BaseValue, Type)

            Return BaseValue
        End Function

        Private Shared Sub DesignViewModelType_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            If Not KsApplication.IsInDesignMode Then
                Exit Sub
            End If

            Dim Self = DirectCast(D, FrameworkElement)

            'Dim OldValue = DirectCast(E.OldValue, Type)
            Dim NewValue = DirectCast(E.NewValue, Type)

            Dim ViewModel = DirectCast(NewValue.CreateInstance(), ViewModel)

            Self.DataContext = ViewModel
            SetViewModel(Self, ViewModel)
            ViewModel.View = DirectCast(Self, Control)
        End Sub

        Public Shared Function GetDesignViewModelType(ByVal Element As DependencyObject) As Type
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(DesignViewModelTypeProperty), Type)
        End Function

        Public Shared Sub SetDesignViewModelType(ByVal Element As DependencyObject, ByVal Value As Type)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(DesignViewModelTypeProperty, Value)
        End Sub
#End Region

#Region "IndexInParent Property"
        Public Shared ReadOnly IndexInParentProperty As DependencyProperty = DependencyProperty.RegisterAttached("IndexInParent", GetType(Integer), GetType(Utils), New PropertyMetadata(-1, AddressOf IndexInParent_Changed, AddressOf IndexInParent_Coerce))

        Private Shared Function IndexInParent_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = TryCast(D, DependencyObject)
            'If Self Is Nothing Then
            '    Return IndexInParentProperty.DefaultMetadata.DefaultValue
            'End If

            Dim Parent = LogicalTreeHelper.GetParent(Self)
            If Not CheckedParents.Add(Parent) Then 'OrElse Not InCheckParents.Add(Parent) Then
                Return BaseValue
            End If

            Dim Res = -1
            Dim I = 0
            For Each C As DependencyObject In LogicalTreeHelper.GetChildren(Parent)
                If Self Is C Then
                    Assert.True(Res = -1)
                    Res = I
                Else
                    SetIndexInParent(C, I)
                End If
                I += 1
            Next

            Assert.False(Res = -1)

            'Assert.True(InCheckParents.Remove(Parent))

            'Dim Value = DirectCast(BaseValue, Integer)

            Return Res
        End Function

        Private Shared Sub IndexInParent_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            'Dim Self = DirectCast(D, DependencyObject)

            'Dim OldValue = DirectCast(E.OldValue, Integer)
            'Dim NewValue = DirectCast(E.NewValue, Integer)
        End Sub

        Public Shared Function GetIndexInParent(ByVal Element As DependencyObject) As Integer
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(IndexInParentProperty), Integer)
        End Function

        Public Shared Sub SetIndexInParent(ByVal Element As DependencyObject, ByVal Value As Integer)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(IndexInParentProperty, Value)
        End Sub

        'Private Shared ReadOnly InCheckParents As HashSet(Of DependencyObject) = New HashSet(Of DependencyObject)()
        Private Shared ReadOnly CheckedParents As HashSet(Of DependencyObject) = New HashSet(Of DependencyObject)()
#End Region

#Region "UpTPProp1 Property"
        Public Shared ReadOnly UpTPProp1Property As DependencyProperty = DependencyProperty.RegisterAttached("UpTPProp1", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing, AddressOf UpTPProp1_Changed))

        Private Shared Sub UpTPProp1_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, FrameworkElement)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            Dim NewValue = DirectCast(E.NewValue, Object)

            If Self.TemplatedParent IsNot Nothing Then
                SetUpTPProp1(Self.TemplatedParent, NewValue)
            End If
        End Sub

        Public Shared Function GetUpTPProp1(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(UpTPProp1Property), Object)
        End Function

        Public Shared Sub SetUpTPProp1(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(UpTPProp1Property, Value)
        End Sub
#End Region

#Region "UpTPProp2 Property"
        Public Shared ReadOnly UpTPProp2Property As DependencyProperty = DependencyProperty.RegisterAttached("UpTPProp2", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing, AddressOf UpTPProp2_Changed))

        Private Shared Sub UpTPProp2_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, FrameworkElement)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            Dim NewValue = DirectCast(E.NewValue, Object)

            If Self.TemplatedParent IsNot Nothing Then
                SetUpTPProp2(Self.TemplatedParent, NewValue)
            End If
        End Sub

        Public Shared Function GetUpTPProp2(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(UpTPProp2Property), Object)
        End Function

        Public Shared Sub SetUpTPProp2(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(UpTPProp2Property, Value)
        End Sub
#End Region

#Region "UpTPProp3 Property"
        Public Shared ReadOnly UpTPProp3Property As DependencyProperty = DependencyProperty.RegisterAttached("UpTPProp3", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing, AddressOf UpTPProp3_Changed))

        Private Shared Sub UpTPProp3_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, FrameworkElement)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            Dim NewValue = DirectCast(E.NewValue, Object)

            If Self.TemplatedParent IsNot Nothing Then
                SetUpTPProp3(Self.TemplatedParent, NewValue)
            End If
        End Sub

        Public Shared Function GetUpTPProp3(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(UpTPProp3Property), Object)
        End Function

        Public Shared Sub SetUpTPProp3(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(UpTPProp3Property, Value)
        End Sub
#End Region

#Region "Prop1 Property"
        Public Shared ReadOnly Prop1Property As DependencyProperty = DependencyProperty.RegisterAttached("Prop1", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing))

        Public Shared Function GetProp1(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(Prop1Property), Object)
        End Function

        Public Shared Sub SetProp1(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(Prop1Property, Value)
        End Sub
#End Region

#Region "Prop2 Property"
        Public Shared ReadOnly Prop2Property As DependencyProperty = DependencyProperty.RegisterAttached("Prop2", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing))

        Public Shared Function GetProp2(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(Prop2Property), Object)
        End Function

        Public Shared Sub SetProp2(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(Prop2Property, Value)
        End Sub
#End Region

#Region "Prop3 Property"
        Public Shared ReadOnly Prop3Property As DependencyProperty = DependencyProperty.RegisterAttached("Prop3", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing))

        Public Shared Function GetProp3(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(Prop3Property), Object)
        End Function

        Public Shared Sub SetProp3(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(Prop3Property, Value)
        End Sub
#End Region

#Region "Prop4 Property"
        Public Shared ReadOnly Prop4Property As DependencyProperty = DependencyProperty.RegisterAttached("Prop4", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing))

        Public Shared Function GetProp4(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(Prop4Property), Object)
        End Function

        Public Shared Sub SetProp4(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(Prop4Property, Value)
        End Sub
#End Region

#Region "Prop5 Property"
        Public Shared ReadOnly Prop5Property As DependencyProperty = DependencyProperty.RegisterAttached("Prop5", GetType(Object), GetType(Utils), New PropertyMetadata(Nothing))

        Public Shared Function GetProp5(ByVal Element As DependencyObject) As Object
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Return DirectCast(Element.GetValue(Prop5Property), Object)
        End Function

        Public Shared Sub SetProp5(ByVal Element As DependencyObject, ByVal Value As Object)
            If Element Is Nothing Then
                Throw New ArgumentNullException(NameOf(Element))
            End If

            Element.SetValue(Prop5Property, Value)
        End Sub
#End Region

#Region "SnapsToDevicePixels Property"
        Public Shared ReadOnly SnapsToDevicePixelsProperty As DependencyProperty = DependencyProperty.RegisterAttached("SnapsToDevicePixels", GetType(Boolean), GetType(Utils), New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.Inherits, AddressOf SnapsToDevicePixels_Changed, AddressOf SnapsToDevicePixels_Coerce))

        Private Shared Function SnapsToDevicePixels_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = TryCast(D, UIElement)
            'If Self Is Nothing Then
            '    Return SnapsToDevicePixelsProperty.DefaultMetadata.DefaultValue
            'End If

            'Dim Value = DirectCast(BaseValue, Boolean)

            Return BaseValue
        End Function

        Private Shared Sub SnapsToDevicePixels_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = TryCast(D, UIElement)

            If Self Is Nothing Then
                Exit Sub
            End If

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            Dim NewValue = DirectCast(E.NewValue, Boolean)

            Self.SnapsToDevicePixels = NewValue
        End Sub

        Public Shared Function GetSnapsToDevicePixels(ByVal Element As DependencyObject) As Boolean
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(SnapsToDevicePixelsProperty), Boolean)
        End Function

        Public Shared Sub SetSnapsToDevicePixels(ByVal Element As DependencyObject, ByVal Value As Boolean)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(SnapsToDevicePixelsProperty, Value)
        End Sub
#End Region

#Region "Foreground Property"
        Public Shared ReadOnly ForegroundProperty As DependencyProperty = DependencyProperty.RegisterAttached("Foreground", GetType(Media.Brush), GetType(Utils), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.Inherits, AddressOf Foreground_Changed, AddressOf Foreground_Coerce))

        Private Shared Function Foreground_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = TryCast(D, UIElement)
            'If Self Is Nothing Then
            '    Return ForegroundProperty.DefaultMetadata.DefaultValue
            'End If

            'Dim Value = DirectCast(BaseValue, Media.Brush)

            Return BaseValue
        End Function

        Private Shared Sub Foreground_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = TryCast(D, UIElement)

            If Self Is Nothing Then
                Exit Sub
            End If

            'Dim OldValue = DirectCast(E.OldValue, Media.Brush)
            Dim NewValue = DirectCast(E.NewValue, Media.Brush)

            Documents.TextElement.SetForeground(Self, NewValue)

            Self = TryCast(TryCast(Self, ContentPresenter)?.Content, UIElement)
            If Self Is Nothing Then
                Exit Sub
            End If
            Documents.TextElement.SetForeground(Self, NewValue)
        End Sub

        Public Shared Function GetForeground(ByVal Element As DependencyObject) As Media.Brush
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(ForegroundProperty), Media.Brush)
        End Function

        Public Shared Sub SetForeground(ByVal Element As DependencyObject, ByVal Value As Media.Brush)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(ForegroundProperty, Value)
        End Sub
#End Region

#Region "DescriptionToolTip Property"
        Public Shared ReadOnly DescriptionToolTipProperty As DependencyProperty = DependencyProperty.RegisterAttached("DescriptionToolTip", GetType(String), GetType(Utils), New PropertyMetadata(Nothing, AddressOf DescriptionToolTip_Changed, AddressOf DescriptionToolTip_Coerce))

        Private Shared Function DescriptionToolTip_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = TryCast(D, FrameworkElement)
            If Self Is Nothing Then
                Return DescriptionToolTipProperty.DefaultMetadata.DefaultValue
            End If

            Dim Value = DirectCast(BaseValue, String)

            Return BaseValue
        End Function

        Private Shared Sub DescriptionToolTip_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, FrameworkElement)

            Dim OldValue = DirectCast(E.OldValue, String)
            Dim NewValue = DirectCast(E.NewValue, String)

            Self.ToolTip = New Controls.ToolTip() With {.Text = "Desc@" & If(NewValue, "")}
        End Sub

        Public Shared Function GetDescriptionToolTip(ByVal Element As DependencyObject) As String
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(DescriptionToolTipProperty), String)
        End Function

        Public Shared Sub SetDescriptionToolTip(ByVal Element As DependencyObject, ByVal Value As String)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(DescriptionToolTipProperty, Value)
        End Sub
#End Region

#Region "ToolTip Property"
        Public Shared ReadOnly ToolTipProperty As DependencyProperty = DependencyProperty.RegisterAttached("ToolTip", GetType(String), GetType(Utils), New PropertyMetadata(Nothing, AddressOf ToolTip_Changed, AddressOf ToolTip_Coerce))

        Private Shared Function ToolTip_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = TryCast(D, FrameworkElement)
            If Self Is Nothing Then
                Return ToolTipProperty.DefaultMetadata.DefaultValue
            End If

            Dim Value = DirectCast(BaseValue, String)

            Return BaseValue
        End Function

        Private Shared Sub ToolTip_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, FrameworkElement)

            Dim OldValue = DirectCast(E.OldValue, String)
            Dim NewValue = DirectCast(E.NewValue, String)

            Self.ToolTip = New Controls.ToolTip() With {.Text = If(NewValue, "")}
        End Sub

        Public Shared Function GetToolTip(ByVal Element As DependencyObject) As String
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(ToolTipProperty), String)
        End Function

        Public Shared Sub SetToolTip(ByVal Element As DependencyObject, ByVal Value As String)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(ToolTipProperty, Value)
        End Sub
#End Region

        Public Const [True] As Boolean = True
        Public Const [False] As Boolean = False

    End Class

End Namespace

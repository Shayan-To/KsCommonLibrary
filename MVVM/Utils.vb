Namespace MVVM

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
                Throw New ArgumentNullException("Element")
            End If

            Return DirectCast(Element.GetValue(ViewModelProperty), ViewModel)
        End Function

        Public Shared Sub SetViewModel(ByVal Element As DependencyObject, ByVal Value As ViewModel)
            If Element Is Nothing Then
                Throw New ArgumentNullException("Element")
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

            Dim ViewModel = DirectCast(NewValue.GetConstructor(Utilities.Typed(Of Type).EmptyArray).Invoke(Utilities.Typed(Of Object).EmptyArray), ViewModel)

            Self.DataContext = ViewModel
            SetViewModel(Self, ViewModel)
            ViewModel.View = DirectCast(Self, Control)
        End Sub

        Public Shared Function GetDesignViewModelType(ByVal Element As DependencyObject) As Type
            If Element Is Nothing Then
                Throw New ArgumentNullException("Element")
            End If

            Return DirectCast(Element.GetValue(DesignViewModelTypeProperty), Type)
        End Function

        Public Shared Sub SetDesignViewModelType(ByVal Element As DependencyObject, ByVal Value As Type)
            If Element Is Nothing Then
                Throw New ArgumentNullException("Element")
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
                Throw New ArgumentNullException("Element")
            End If

            Return DirectCast(Element.GetValue(IndexInParentProperty), Integer)
        End Function

        Public Shared Sub SetIndexInParent(ByVal Element As DependencyObject, ByVal Value As Integer)
            If Element Is Nothing Then
                Throw New ArgumentNullException("Element")
            End If

            Element.SetValue(IndexInParentProperty, Value)
        End Sub

        'Private Shared ReadOnly InCheckParents As HashSet(Of DependencyObject) = New HashSet(Of DependencyObject)()
        Private Shared ReadOnly CheckedParents As HashSet(Of DependencyObject) = New HashSet(Of DependencyObject)()
#End Region

    End Class

End Namespace

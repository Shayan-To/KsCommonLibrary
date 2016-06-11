Imports System.Windows.Media.Animation

Namespace Controls

    Public Class Obj
        Inherits FrameworkContentElement ' ToDo For the binding on the Obj to work, this was forced to change to a FrameworkElement or a FrameworkContentElement. Otherwise, the AddLogicalChild on the TextBlock has no effect and DataContext, TemplatedParent, ElementName, FindAncestor, etc. will not work in bindings on the Obj in the Obj list. Find out why this does not work, and remove this unnecessary base class. (why bindings work on Brush, RotateTransform, etc.?)

        Friend Sub ReportParent(ByVal Parent As TextBlock)
            Me._TextBlock = Parent
        End Sub

#Region "Obj Property"
        Public Shared ReadOnly ObjProperty As DependencyProperty = DependencyProperty.Register("Obj", GetType(Object), GetType(Obj), New PropertyMetadata(Nothing, AddressOf Obj_Changed, AddressOf Obj_Coerce))

        Private Shared Function Obj_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, Obj)

            'Dim Value = DirectCast(BaseValue, Object)

            Return BaseValue
        End Function

        Private Shared Sub Obj_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, Obj)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            'Dim NewValue = DirectCast(E.NewValue, Object)

            Self.TextBlock?.ReportObjChanged()
        End Sub

        Public Property Obj As Object
            Get
                Return DirectCast(Me.GetValue(ObjProperty), Object)
            End Get
            Set(ByVal value As Object)
                Me.SetValue(ObjProperty, value)
            End Set
        End Property
#End Region

#Region "TextBlock Property"
        Private _TextBlock As TextBlock

        Public ReadOnly Property TextBlock As TextBlock
            Get
                Return Me._TextBlock
            End Get
        End Property
#End Region

    End Class

End Namespace

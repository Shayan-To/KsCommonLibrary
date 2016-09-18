Imports System.Collections.Specialized

Namespace MVVM

    Public Class CollectionObserver(Of T)

        Public Sub New()

        End Sub

        Private Sub Collection_CollectionChanged(ByVal Sender As Object, ByVal E As NotifyCollectionChangedEventArgs)
            Select Case E.Action
                Case NotifyCollectionChangedAction.Add
                    For Each I As T In E.NewItems
                        Me.OnElementGotIn(New ElementEventArgs(Of T)(I))
                    Next
                Case NotifyCollectionChangedAction.Remove
                    For Each I As T In E.OldItems
                        Me.OnElementGotOut(New ElementEventArgs(Of T)(I))
                    Next
                Case NotifyCollectionChangedAction.Move
                Case NotifyCollectionChangedAction.Replace
                    For Each I As T In E.OldItems
                        Me.OnElementGotOut(New ElementEventArgs(Of T)(I))
                    Next
                    For Each I As T In E.NewItems
                        Me.OnElementGotIn(New ElementEventArgs(Of T)(I))
                    Next
                Case NotifyCollectionChangedAction.Reset
                    For Each I As T In Me.Clone
                        Me.OnElementGotOut(New ElementEventArgs(Of T)(I))
                    Next
                    For Each I As T In Me.Collection
                        Me.OnElementGotIn(New ElementEventArgs(Of T)(I))
                    Next
            End Select

            Me.OnCollectionChanged()

            Select Case E.Action
                Case NotifyCollectionChangedAction.Add
                    For I As Integer = 0 To E.NewItems.Count - 1
                        Me.Clone.Insert(E.NewStartingIndex + I, E.NewItems(I))
                    Next
                Case NotifyCollectionChangedAction.Remove
                    For I As Integer = E.OldItems.Count - 1 To 0 Step -1
                        Me.Clone.RemoveAt(E.OldStartingIndex + I)
                    Next
                Case NotifyCollectionChangedAction.Move
                    If E.NewStartingIndex < E.OldStartingIndex Then
                        For I As Integer = 0 To E.NewItems.Count - 1
                            Me.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I)
                        Next
                    Else
                        For I As Integer = E.NewItems.Count - 1 To 0 Step -1
                            Me.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I)
                        Next
                    End If
                Case NotifyCollectionChangedAction.Replace
                    For I As Integer = 0 To E.NewItems.Count - 1
                        Me.Clone.Item(E.NewStartingIndex + I) = E.NewItems(I)
                    Next
                Case NotifyCollectionChangedAction.Reset
                    Me.Clone.Clear()
                    If Me.Collection IsNot Nothing Then
                        Me.Clone.AddRange(Me.Collection)
                    End If
            End Select
        End Sub

#Region "ElementGotIn Event"
        Public Event ElementGotIn As EventHandler(Of ElementEventArgs(Of T))

        Protected Overridable Sub OnElementGotIn(ByVal E As ElementEventArgs(Of T))
            RaiseEvent ElementGotIn(Me, E)
        End Sub
#End Region

#Region "ElementGotOut Event"
        Public Event ElementGotOut As EventHandler(Of ElementEventArgs(Of T))

        Protected Overridable Sub OnElementGotOut(ByVal E As ElementEventArgs(Of T))
            RaiseEvent ElementGotOut(Me, E)
        End Sub
#End Region

#Region "CollectionChanged Event"
        Public Event CollectionChanged As EventHandler

        Protected Overridable Sub OnCollectionChanged()
            RaiseEvent CollectionChanged(Me, EventArgs.Empty)
        End Sub
#End Region

#Region "Collection Property"
        Private _Collection As IEnumerable

        Public Property Collection As IEnumerable
            Get
                Return Me._Collection
            End Get
            Set(ByVal Value As IEnumerable)
                Dim Obs = TryCast(Me._Collection, INotifyCollectionChanged)
                If Obs IsNot Nothing Then
                    RemoveHandler Obs.CollectionChanged, Me._Collection_CollectionChanged
                End If

                Me._Collection = Value

                Obs = TryCast(Me._Collection, INotifyCollectionChanged)
                If Obs IsNot Nothing Then
                    AddHandler Obs.CollectionChanged, Me._Collection_CollectionChanged
                End If

                If Me.AssumeSettingOfCollectionAsReset Then
                    Me.Collection_CollectionChanged(Me.Collection, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
                Else
                    Me.Clone.Clear()
                    If Me.Collection IsNot Nothing Then
                        Me.Clone.AddRange(Me.Collection)
                    End If
                End If
            End Set
        End Property
#End Region

#Region "AssumeSettingOfCollectionAsReset Property"
        Private _AssumeSettingOfCollectionAsReset As Boolean = True

        Public Property AssumeSettingOfCollectionAsReset As Boolean
            Get
                Return Me._AssumeSettingOfCollectionAsReset
            End Get
            Set(ByVal Value As Boolean)
                Me._AssumeSettingOfCollectionAsReset = Value
            End Set
        End Property
#End Region

        Private ReadOnly Clone As List(Of Object) = New List(Of Object)()
        Private ReadOnly _Collection_CollectionChanged As NotifyCollectionChangedEventHandler = New NotifyCollectionChangedEventHandler(AddressOf Me.Collection_CollectionChanged)

    End Class

    Public Class ElementEventArgs(Of T)
        Inherits EventArgs

        Public Sub New(ByVal Element As T)
            Me._Element = Element
        End Sub

#Region "Element Property"
        Private ReadOnly _Element As T

        Public ReadOnly Property Element As T
            Get
                Return Me._Element
            End Get
        End Property
#End Region

    End Class

    '    Public Class ElementMovedEventArgs(Of T)
    '        Inherits ElementEventArgs(Of T)

    '        Public Sub New(ByVal Element As T, ByVal OldIndex As Integer, ByVal NewIndex As Integer)
    '            MyBase.New(Element)
    '            Me._OldIndex = OldIndex
    '            Me._NewIndex = NewIndex
    '        End Sub

    '#Region "NewIndex Property"
    '        Private ReadOnly _NewIndex As Integer

    '        Public ReadOnly Property NewIndex As Integer
    '            Get
    '                Return Me._NewIndex
    '            End Get
    '        End Property
    '#End Region

    '#Region "OldIndex Property"
    '        Private ReadOnly _OldIndex As Integer

    '        Public ReadOnly Property OldIndex As Integer
    '            Get
    '                Return Me._OldIndex
    '            End Get
    '        End Property
    '#End Region

    '    End Class

End Namespace

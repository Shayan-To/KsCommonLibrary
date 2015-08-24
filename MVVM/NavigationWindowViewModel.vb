Namespace MVVM

    Public Class NavigationWindowViewModel
        Inherits WindowViewModel

        Public Sub New(ByVal Window As Window, ByVal ContentFrame As ContentControl)
            MyBase.New(Window)

            Me._PagesStack = New Stack(Of NavigablePageViewModelBase)()
            Me._PagesStackEnabled = True
            Me._ContentFrame = ContentFrame
        End Sub

        Public Sub New(ByVal Window As Window)
            Me.New(Window, Window)
        End Sub

        Public Sub New()
            Me._PagesStack = New Stack(Of NavigablePageViewModelBase)()
            Me._PagesStackEnabled = True
        End Sub

#Region "Navigation Logic"
        Public Sub NavigateTo(ByVal PageViewModel As NavigablePageViewModelBase)
            Dim OldPage As NavigablePageViewModelBase

            If PageViewModel.NavigationWindow IsNot Me Then
                Throw New ArgumentException("The NavigationWindow of the PageViewModel specified should be this insance for this method to work.")
            End If

            OldPage = Me.CurrentPage

            If OldPage IsNot Nothing Then
                OldPage.OnNavigatingFrom(EventArgs.Empty)
            End If
            PageViewModel.OnNavigatingTo(EventArgs.Empty)

            If Not Me._PagesStackEnabled Then
                Me._PagesStack.Clear()
            End If

            Me._PagesStack.Push(PageViewModel)
            Me.NavigateToInternal(PageViewModel)

            If OldPage IsNot Nothing Then
                OldPage.OnNavigatedFrom(EventArgs.Empty)
            End If
            PageViewModel.OnNavigatedTo(EventArgs.Empty)
        End Sub

        Public Sub NavigateBack()
            Dim OldPage, NewPage As NavigablePageViewModelBase

            If Not Me._PagesStackEnabled Then
                Throw New NotSupportedException("PagesStack is desabled.")
            End If

            If Me._PagesStack.Count < 2 Then
                Throw New Exception("There are no pages on the stack to navigate back.")
            End If

            OldPage = Me._PagesStack.Pop()
            NewPage = Me._PagesStack.Peek()
            Me._PagesStack.Push(OldPage)

            OldPage.OnNavigatingFrom(EventArgs.Empty)
            NewPage.OnNavigatingTo(EventArgs.Empty)

            Me._PagesStack.Pop()
            Me.NavigateToInternal(NewPage)

            OldPage.OnNavigatedFrom(EventArgs.Empty)
            NewPage.OnNavigatedTo(EventArgs.Empty)
        End Sub

        Protected Sub NavigateToInternal(ByVal PageViewModel As NavigablePageViewModelBase)
            Me._ContentFrame.Content = PageViewModel.Page
        End Sub
#End Region

#Region "PagesStack Property"
        Private ReadOnly _PagesStack As Stack(Of NavigablePageViewModelBase)

        Public ReadOnly Property PagesStack As Stack(Of NavigablePageViewModelBase)
            Get
                If Not Me._PagesStackEnabled Then
                    Throw New Exception("PagesStack is disabled.")
                End If
                Return Me._PagesStack
            End Get
        End Property
#End Region

#Region "PagesStackEnabled Property"
        Private _PagesStackEnabled As Boolean

        Public Property PagesStackEnabled As Boolean
            Get
                Return Me._PagesStackEnabled
            End Get
            Set(ByVal Value As Boolean)
                Me._PagesStackEnabled = Value
            End Set
        End Property
#End Region

#Region "CurrentPage Property"
        Public Property CurrentPage As NavigablePageViewModelBase
            Get
                If Me._PagesStack.Count = 0 Then
                    Return Nothing
                End If
                Return Me._PagesStack.Peek()
            End Get
            Set(ByVal Value As NavigablePageViewModelBase)
                Me.NavigateTo(Value)
            End Set
        End Property
#End Region

#Region "ContentFrame Property"
        Private ReadOnly _ContentFrame As ContentControl

        Public ReadOnly Property ContentFrame As ContentControl
            Get
                Return Me._ContentFrame
            End Get
        End Property
#End Region

    End Class

End Namespace

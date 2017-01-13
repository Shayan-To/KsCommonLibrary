Imports Ks.Common.MVVM

Namespace Common.Controls

    <StyleTypedProperty(Property:=NameOf(CheckListBox.ItemContainerStyle), StyleTargetType:=GetType(CheckListBoxItem))>
    Public Class CheckListBox
        Inherits ItemsControl

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(CheckListBox), New FrameworkPropertyMetadata(GetType(CheckListBox)))
            'SelectionModeProperty.OverrideMetadata(GetType(ListBox), New FrameworkPropertyMetadata(AddressOf SelectionMode_Changed))
        End Sub

        Public Sub New()
            AddHandler Me.CollectionObserver.ElementGotIn, AddressOf Me.CheckedItems_ItemGotIn
            AddHandler Me.CollectionObserver.ElementGotOut, AddressOf Me.CheckedItems_ItemGotOut
        End Sub

        'Private Shared Sub SelectionMode_Changed(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        '    Dim Self = DirectCast(d, ListBox)

        '    Dim NewValue = DirectCast(e.NewValue, SelectionMode)

        '    If NewValue <> SelectionMode.Single Then
        '        'Self.ItemContainerGenerator.
        '    End If
        'End Sub

        'Private Sub SelectedItemsSource_Changed(sender As Object, e As NotifyCollectionChangedEventArgs)
        '    ' ToDo Listen for changes in the SelectedItems as well and apply them to SelectedItemsSource.
        '    If Me.IsDirty Then
        '        Exit Sub
        '    End If

        '    Dim Items = DirectCast(Me.SelectedItems, IList(Of Object))
        '    Select Case e.Action
        '        Case NotifyCollectionChangedAction.Add
        '            For Each I In e.NewItems
        '                Items.Add(I)
        '            Next
        '        Case NotifyCollectionChangedAction.Remove
        '            For Each I In e.OldItems
        '                Items.Remove(I)
        '            Next
        '        Case NotifyCollectionChangedAction.Replace
        '            For Each I In e.OldItems
        '                Items.Remove(I)
        '            Next
        '            For Each I In e.NewItems
        '                Items.Add(I)
        '            Next
        '        Case NotifyCollectionChangedAction.Reset
        '            Me.SelectedItemsSource_Reset()
        '    End Select
        'End Sub

        'Private Sub SelectedItemsSource_Reset()
        '    Dim Items = Me.SelectedItems
        '    Items.Clear()
        '    Items.AddRange(Me.SelectedItemsSource)
        'End Sub

        'Private Shadows ReadOnly Property SelectedItems As IList
        '    Get
        '        Return MyBase.SelectedItems
        '    End Get
        'End Property

        '#Region "SelectedItemsSource Property"
        'Public Shared ReadOnly SelectedItemsSourceProperty As DependencyProperty = DependencyProperty.Register("SelectedItemsSource", GetType(IEnumerable), GetType(ListBox), New PropertyMetadata(Nothing, AddressOf SelectedItemsSource_Changed, AddressOf SelectedItemsSource_Coerce))

        'Private Shared Function SelectedItemsSource_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
        '    Dim Self = DirectCast(D, ListBox)

        '    Dim Value = DirectCast(BaseValue, IEnumerable)

        '    Return BaseValue
        'End Function

        'Private Shared Sub SelectedItemsSource_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        '    Dim Self = DirectCast(D, ListBox)

        '    Dim OldValue = DirectCast(E.OldValue, IEnumerable)
        '    Dim NewValue = DirectCast(E.NewValue, IEnumerable)

        '    If NewValue IsNot Nothing And Self.SelectionMode = SelectionMode.Single Then
        '        Verify.Fail("Cannot set SelectedItemsSource to a non-null value when SelectionMode is Single.")
        '    End If

        '    Dim T = TryCast(OldValue, INotifyCollectionChanged)
        '    If T IsNot Nothing Then
        '        RemoveHandler T.CollectionChanged, AddressOf Self.SelectedItemsSource_Changed
        '    End If
        '    T = TryCast(NewValue, INotifyCollectionChanged)
        '    If T IsNot Nothing Then
        '        AddHandler T.CollectionChanged, AddressOf Self.SelectedItemsSource_Changed
        '    End If

        '    Self.SelectedItemsSource_Reset()
        'End Sub

        'Public Property SelectedItemsSource As IEnumerable
        '    Get
        '        Return DirectCast(Me.GetValue(SelectedItemsSourceProperty), IEnumerable)
        '    End Get
        '    Set(ByVal value As IEnumerable)
        '        Me.SetValue(SelectedItemsSourceProperty, value)
        '    End Set
        'End Property
        '#End Region

        'Private IsDirty As Boolean

        Protected Overrides Function IsItemItsOwnContainerOverride(item As Object) As Boolean
            Return TypeOf item Is CheckListBoxItem
        End Function

        Protected Overrides Function GetContainerForItemOverride() As DependencyObject
            Return New CheckListBoxItem() With {.ParentListBox = Me}
        End Function

        Protected Overrides Sub ClearContainerForItemOverride(element As DependencyObject, item As Object)
            Dim Container = DirectCast(element, CheckListBoxItem)
            Me.Dirty = True
            Container.IsChecked = False
            Me.Dirty = False
            MyBase.ClearContainerForItemOverride(element, item)
        End Sub

        Protected Overrides Sub PrepareContainerForItemOverride(element As DependencyObject, item As Object)
            Dim Container = DirectCast(element, CheckListBoxItem)
            Me.Dirty = True
            Container.IsChecked = If(Me.CheckedItems?.Contains(item), False)
            Me.Dirty = False
            MyBase.PrepareContainerForItemOverride(element, item)
        End Sub

        Friend Sub ReportCheckStateChanged(ByVal Container As CheckListBoxItem)
            Dim Items = Me.CheckedItems
            If Items Is Nothing Or Me.Dirty Then
                Exit Sub
            End If

            Dim Item = Me.ItemContainerGenerator.ItemFromContainer(Container)
            If Container.IsChecked Then
                Items.Add(Item)
            Else
                Items.Remove(Item)
            End If
        End Sub

        Private Sub CheckedItems_ItemGotOut(sender As Object, e As ElementEventArgs(Of Object))
            Dim Items = Me.CheckedItems
            If Items Is Nothing Or Me.Dirty Then
                Exit Sub
            End If

            Me.Dirty = True
            Dim Container = DirectCast(Me.ItemContainerGenerator.ContainerFromItem(e.Element), CheckListBoxItem)
            If Container IsNot Nothing Then
                Container.IsChecked = False
            End If
            Me.Dirty = False
        End Sub

        Private Sub CheckedItems_ItemGotIn(sender As Object, e As ElementEventArgs(Of Object))
            Dim Items = Me.CheckedItems
            If Items Is Nothing Or Me.Dirty Then
                Exit Sub
            End If

            Me.Dirty = True
            Dim Container = DirectCast(Me.ItemContainerGenerator.ContainerFromItem(e.Element), CheckListBoxItem)
            If Container IsNot Nothing Then
                Container.IsChecked = True
            End If
            Me.Dirty = False
        End Sub

        Private Sub CheckedItems_Reset()
            Me.Dirty = True
            Dim ICGen = Me.ItemContainerGenerator
            Dim ChItems = Me.CheckedItems
            Dim Items = ICGen.Items

            For Each I In Items
                Dim Container = DirectCast(ICGen.ContainerFromItem(I), CheckListBoxItem)
                If Container IsNot Nothing Then
                    Container.IsChecked = False
                End If
            Next
            If ChItems IsNot Nothing Then
                Dim I = 0
                Do While I < ChItems.Count
                    Dim Container = DirectCast(ICGen.ContainerFromItem(ChItems.Item(I)), CheckListBoxItem)
                    If Container IsNot Nothing Then
                        Container.IsChecked = True
                    Else
                        If Not Items.Contains(ChItems.Item(I)) Then
                            ChItems.RemoveAt(I)
                            Continue Do
                        End If
                    End If
                    I += 1
                Loop
            End If
            Me.Dirty = False
        End Sub

#Region "CheckedItems Property"
        Public Shared ReadOnly CheckedItemsProperty As DependencyProperty = DependencyProperty.Register("CheckedItems", GetType(IList), GetType(CheckListBox), New PropertyMetadata(Nothing, AddressOf CheckedItems_Changed, AddressOf CheckedItems_Coerce))

        Private Shared Function CheckedItems_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, CheckListBox)

            'Dim Value = DirectCast(BaseValue, IList)

            Return BaseValue
        End Function

        Private Shared Sub CheckedItems_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, CheckListBox)

            Dim OldValue = DirectCast(E.OldValue, IList)
            Dim NewValue = DirectCast(E.NewValue, IList)

            Self.CollectionObserver.Collection = NewValue
            Self.CheckedItems_Reset()
        End Sub

        Public Property CheckedItems As IList
            Get
                Return DirectCast(Me.GetValue(CheckedItemsProperty), IList)
            End Get
            Set(ByVal value As IList)
                Me.SetValue(CheckedItemsProperty, value)
            End Set
        End Property
#End Region

        Private ReadOnly CollectionObserver As MVVM.CollectionObserver(Of Object) = New MVVM.CollectionObserver(Of Object)() With {.AssumeSettingOfCollectionAsReset = False}
        Private Dirty As Boolean

    End Class

    Public Class CheckListBoxItem
        Inherits ContentControl

#Region "IsChecked Property"
        Public Shared ReadOnly IsCheckedProperty As DependencyProperty = DependencyProperty.Register("IsChecked", GetType(Boolean), GetType(CheckListBoxItem), New PropertyMetadata(False, AddressOf IsChecked_Changed, AddressOf IsChecked_Coerce))

        Private Shared Function IsChecked_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, CheckListBoxItem)

            'Dim Value = DirectCast(BaseValue, Boolean)

            Dim ParItems = Self.ParentListBox.CheckedItems
            If ParItems Is Nothing OrElse ParItems.IsReadOnly Then
                Return Self.IsChecked
            End If

            Return BaseValue
        End Function

        Private Shared Sub IsChecked_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, CheckListBoxItem)

            'Dim OldValue = DirectCast(E.OldValue, Boolean)
            'Dim NewValue = DirectCast(E.NewValue, Boolean)

            Self.ParentListBox.ReportCheckStateChanged(Self)
        End Sub

        Public Property IsChecked As Boolean
            Get
                Return DirectCast(Me.GetValue(IsCheckedProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me.SetValue(IsCheckedProperty, value)
            End Set
        End Property
#End Region

#Region "ParentListBox Property"
        Private _ParentListBox As CNullable(Of CheckListBox)

        Public Property ParentListBox As CheckListBox
            Get
                Return Me._ParentListBox.Value
            End Get
            Friend Set(ByVal Value As CheckListBox)
                Verify.False(Me._ParentListBox.HasValue, "Cannot set the ParentListBox property multiple times.")
                Me._ParentListBox = Value
            End Set
        End Property
#End Region

    End Class

End Namespace

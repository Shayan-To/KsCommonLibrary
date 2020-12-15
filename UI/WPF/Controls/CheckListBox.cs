using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

using Ks.Common.MVVM;

namespace Ks.Common.Controls
{
    [StyleTypedProperty(Property = nameof(CheckListBox.ItemContainerStyle), StyleTargetType = typeof(CheckListBoxItem))]
    public class CheckListBox : ItemsControl
    {
        static CheckListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckListBox), new FrameworkPropertyMetadata(typeof(CheckListBox)));
        }

        public CheckListBox()
        {
            this.CollectionObserver.ElementGotIn += this.CheckedItems_ItemGotIn;
            this.CollectionObserver.ElementGotOut += this.CheckedItems_ItemGotOut;
        }

        // Private Shared Sub SelectionMode_Changed(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        // Dim Self = DirectCast(d, ListBox)

        // Dim NewValue = DirectCast(e.NewValue, SelectionMode)

        // If NewValue <> SelectionMode.Single Then
        // 'Self.ItemContainerGenerator.
        // End If
        // End Sub

        // Private Sub SelectedItemsSource_Changed(sender As Object, e As NotifyCollectionChangedEventArgs)
        // ' ToDo Listen for changes in the SelectedItems as well and apply them to SelectedItemsSource.
        // If Me.IsDirty Then
        // Exit Sub
        // End If

        // Dim Items = DirectCast(Me.SelectedItems, IList(Of Object))
        // Select Case e.Action
        // Case NotifyCollectionChangedAction.Add
        // For Each I In e.NewItems
        // Items.Add(I)
        // Next
        // Case NotifyCollectionChangedAction.Remove
        // For Each I In e.OldItems
        // Items.Remove(I)
        // Next
        // Case NotifyCollectionChangedAction.Replace
        // For Each I In e.OldItems
        // Items.Remove(I)
        // Next
        // For Each I In e.NewItems
        // Items.Add(I)
        // Next
        // Case NotifyCollectionChangedAction.Reset
        // Me.SelectedItemsSource_Reset()
        // End Select
        // End Sub

        // Private Sub SelectedItemsSource_Reset()
        // Dim Items = Me.SelectedItems
        // Items.Clear()
        // Items.AddRange(Me.SelectedItemsSource)
        // End Sub

        // Private Shadows ReadOnly Property SelectedItems As IList
        // Get
        // Return MyBase.SelectedItems
        // End Get
        // End Property

        // #Region "SelectedItemsSource Property"
        // Public Shared ReadOnly SelectedItemsSourceProperty As DependencyProperty = DependencyProperty.Register("SelectedItemsSource", GetType(IEnumerable), GetType(ListBox), New PropertyMetadata(Nothing, AddressOf SelectedItemsSource_Changed, AddressOf SelectedItemsSource_Coerce))

        // Private Shared Function SelectedItemsSource_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
        // Dim Self = DirectCast(D, ListBox)

        // Dim Value = DirectCast(BaseValue, IEnumerable)

        // Return BaseValue
        // End Function

        // Private Shared Sub SelectedItemsSource_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        // Dim Self = DirectCast(D, ListBox)

        // Dim OldValue = DirectCast(E.OldValue, IEnumerable)
        // Dim NewValue = DirectCast(E.NewValue, IEnumerable)

        // If NewValue IsNot Nothing And Self.SelectionMode = SelectionMode.Single Then
        // Verify.Fail("Cannot set SelectedItemsSource to a non-null value when SelectionMode is Single.")
        // End If

        // Dim T = TryCast(OldValue, INotifyCollectionChanged)
        // If T IsNot Nothing Then
        // RemoveHandler T.CollectionChanged, AddressOf Self.SelectedItemsSource_Changed
        // End If
        // T = TryCast(NewValue, INotifyCollectionChanged)
        // If T IsNot Nothing Then
        // AddHandler T.CollectionChanged, AddressOf Self.SelectedItemsSource_Changed
        // End If

        // Self.SelectedItemsSource_Reset()
        // End Sub

        // Public Property SelectedItemsSource As IEnumerable
        // Get
        // Return DirectCast(Me.GetValue(SelectedItemsSourceProperty), IEnumerable)
        // End Get
        // Set(ByVal value As IEnumerable)
        // Me.SetValue(SelectedItemsSourceProperty, value)
        // End Set
        // End Property
        // #End Region

        // Private IsDirty As Boolean

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CheckListBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CheckListBoxItem() { ParentListBox = this };
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var Container = (CheckListBoxItem) element;
            this.Dirty = true;
            Container.IsChecked = false;
            this.Dirty = false;
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var Container = (CheckListBoxItem) element;
            this.Dirty = true;
            Container.IsChecked = this.CheckedItems?.Contains(item) ?? false;
            this.Dirty = false;
            base.PrepareContainerForItemOverride(element, item);
        }

        internal void ReportCheckStateChanged(CheckListBoxItem Container)
        {
            var Items = this.CheckedItems;
            if (Items == null | this.Dirty)
            {
                return;
            }

            var Item = this.ItemContainerGenerator.ItemFromContainer(Container);
            if (Container.IsChecked)
            {
                Items.Add(Item);
            }
            else
            {
                Items.Remove(Item);
            }
        }

        private void CheckedItems_ItemGotOut(object sender, ElementEventArgs<object> e)
        {
            var Items = this.CheckedItems;
            if (Items == null | this.Dirty)
            {
                return;
            }

            this.Dirty = true;
            var Container = (CheckListBoxItem) this.ItemContainerGenerator.ContainerFromItem(e.Element);
            if (Container != null)
            {
                Container.IsChecked = false;
            }

            this.Dirty = false;
        }

        private void CheckedItems_ItemGotIn(object sender, ElementEventArgs<object> e)
        {
            var Items = this.CheckedItems;
            if (Items == null | this.Dirty)
            {
                return;
            }

            this.Dirty = true;
            var Container = (CheckListBoxItem) this.ItemContainerGenerator.ContainerFromItem(e.Element);
            if (Container != null)
            {
                Container.IsChecked = true;
            }

            this.Dirty = false;
        }

        private void CheckedItems_Reset()
        {
            this.Dirty = true;
            var ICGen = this.ItemContainerGenerator;
            var ChItems = this.CheckedItems;
            var Items = ICGen.Items;

            foreach (var I in Items)
            {
                var Container = (CheckListBoxItem) ICGen.ContainerFromItem(I);
                if (Container != null)
                {
                    Container.IsChecked = false;
                }
            }
            if (ChItems != null)
            {
                var I = 0;
                while (I < ChItems.Count)
                {
                    var Container = (CheckListBoxItem) ICGen.ContainerFromItem(ChItems[I]);
                    if (Container != null)
                    {
                        Container.IsChecked = true;
                    }
                    else if (!Items.Contains(ChItems[I]))
                    {
                        ChItems.RemoveAt(I);
                        continue;
                    }
                    I += 1;
                }
            }
            this.Dirty = false;
        }

        public static readonly DependencyProperty CheckedItemsProperty = DependencyProperty.Register("CheckedItems", typeof(IList), typeof(CheckListBox), new PropertyMetadata(null, CheckedItems_Changed, CheckedItems_Coerce));

        private static object CheckedItems_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, CheckListBox)

            // Dim Value = DirectCast(BaseValue, IList)

            return BaseValue;
        }

        private static void CheckedItems_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (CheckListBox) D;

            // var OldValue = (IList) E.OldValue;
            var NewValue = (IList) E.NewValue;

            Self.CollectionObserver.Collection = NewValue;
            Self.CheckedItems_Reset();
        }

        public IList CheckedItems
        {
            get => (IList) this.GetValue(CheckedItemsProperty);
            set => this.SetValue(CheckedItemsProperty, value);
        }

        private readonly CollectionObserver<object> CollectionObserver = new CollectionObserver<object>() { AssumeSettingOfCollectionAsReset = false };
        private bool Dirty;
    }

    public class CheckListBoxItem : ContentControl
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(CheckListBoxItem), new PropertyMetadata(false, IsChecked_Changed, IsChecked_Coerce));

        private static object IsChecked_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = (CheckListBoxItem) D;

            // Dim Value = DirectCast(BaseValue, Boolean)

            var ParItems = Self.ParentListBox.CheckedItems;
            if (ParItems == null || ParItems.IsReadOnly)
            {
                return Self.IsChecked;
            }

            return BaseValue;
        }

        private static void IsChecked_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (CheckListBoxItem) D;

            // Dim OldValue = DirectCast(E.OldValue, Boolean)
            // Dim NewValue = DirectCast(E.NewValue, Boolean)

            Self.ParentListBox.ReportCheckStateChanged(Self);
        }

        public bool IsChecked
        {
            get => (bool) this.GetValue(IsCheckedProperty);
            set => this.SetValue(IsCheckedProperty, value);
        }

        private CNullable<CheckListBox> _ParentListBox;

        public CheckListBox ParentListBox
        {
            get => this._ParentListBox.Value;
            internal set
            {
                Verify.False(this._ParentListBox.HasValue, "Cannot set the ParentListBox property multiple times.");
                this._ParentListBox = value;
            }
        }
    }
}

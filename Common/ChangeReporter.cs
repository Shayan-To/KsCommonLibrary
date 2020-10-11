using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Ks.Common
{
    public class ChangeReporter
    {
        private readonly List<object> A = new List<object>();

        public void Add(object Obj)
        {
            INotifyPropertyChanged PropObj;
            INotifyCollectionChanged CollecObj;
            IEnumerable Enumerable;

            if (Obj.GetType().Name == "NotifyingCollection`1")
            {
                this.A.Add(Obj);
            }

            PropObj = Obj as INotifyPropertyChanged;
            if (PropObj != null)
            {
                PropObj.PropertyChanged += this.OnPropertyChanged;
            }

            CollecObj = Obj as INotifyCollectionChanged;
            if (CollecObj != null)
            {
                CollecObj.CollectionChanged += this.OnCollectionChanged;
            }

            Enumerable = Obj as IEnumerable;
            if (Enumerable != null)
            {
                foreach (var O in Enumerable)
                {
                    this.Add(O);
                }
            }
        }

        public void Remove(object Obj)
        {
            INotifyPropertyChanged PropObj;
            INotifyCollectionChanged CollecObj;
            IEnumerable Enumerable;

            PropObj = Obj as INotifyPropertyChanged;
            if (PropObj != null)
            {
                PropObj.PropertyChanged -= this.OnPropertyChanged;
            }

            CollecObj = Obj as INotifyCollectionChanged;
            if (CollecObj != null)
            {
                CollecObj.CollectionChanged -= this.OnCollectionChanged;
            }

            Enumerable = Obj as IEnumerable;
            if (Enumerable != null)
            {
                foreach (var O in Enumerable)
                {
                    this.Remove(O);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnObjectChanged();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (var O in e.OldItems)
                    {
                        this.Remove(O);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var O in e.NewItems)
                    {
                        this.Add(O);
                    }
                }
            }
            this.OnObjectChanged();
        }

        public event EventHandler<EventArgs> ObjectChanged;

        protected virtual void OnObjectChanged()
        {
            ObjectChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

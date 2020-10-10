using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using Ks.Common.MVVM;

namespace Ks.Common
{
    [TypeDescriptionProvider(typeof(ValueBagTypeDescriptionProvider))]
    public class ValueBag<T> : BindableBase, IDictionary<string, T>, IDictionary, IFormattable
    {
        public int Count
        {
            get
            {
                return this.Dic.Count;
            }
        }

        private bool IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<string, T>>) this.Dic).IsReadOnly;
            }
        }

        public T this[string Key]
        {
            get
            {
                return this.Dic[Key];
            }
            set
            {
                this.Dic[Key] = value;
                this.NotifyPropertyChanged(Key);
            }
        }

        ICollection<string> IDictionary<string, T>.Keys
        {
            get
            {
                return this.Dic.Keys;
            }
        }

        public Dictionary<string, T>.KeyCollection Keys
        {
            get
            {
                return this.Dic.Keys;
            }
        }

        ICollection<T> IDictionary<string, T>.Values
        {
            get
            {
                return this.Dic.Values;
            }
        }

        public Dictionary<string, T>.ValueCollection Values
        {
            get
            {
                return this.Dic.Values;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[(string) key];
            }
            set
            {
                this[(string) key] = (T) value;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return this.Values;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return this.IsReadOnly;
            }
        }

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly
        {
            get
            {
                return this.IsReadOnly;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> Item)
        {
            this.Dic.Add(Item.Key, Item.Value);
            this.NotifyPropertyChanged(Item.Key);
        }

        public void Add(string Key, T Value)
        {
            this.Dic.Add(Key, Value);
            this.NotifyPropertyChanged(Key);
        }

        public void Clear()
        {
            this.Dic.Clear();
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>) this.Dic).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>) this.Dic).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.Dic.ContainsKey(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Dic.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            return this.Dic.GetEnumerator();
        }

        public Dictionary<string, T>.Enumerator GetEnumerator()
        {
            return this.Dic.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>) this.Dic).Remove(item);
        }

        public bool Remove(string key)
        {
            return this.Dic.Remove(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return this.Dic.TryGetValue(key, out value);
        }

        public override string ToString()
        {
            return this.ToString("", Utilities.Text.CurruntFormatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var R = new System.Text.StringBuilder("{");
            var Bl = true;
            foreach (var KV in this)
            {
                if (Bl)
                {
                    Bl = false;
                }
                else
                {
                    R.Append(", ");
                }

                R.Append(KV.Key).Append(" : ").Append(string.Format(formatProvider, "{0:" + format + "}", KV.Value));
            }

            return R.Append('}').ToString();
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((string) key);
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add((string) key, (T) value);
        }

        void IDictionary.Clear()
        {
            this.Clear();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            this.Remove((string) key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary) this.Dic).CopyTo(array, index);
        }

        private readonly Dictionary<string, T> Dic = new Dictionary<string, T>();
    }

    internal class ValueBagTypeDescriptionProvider : TypeDescriptionProvider
    {
        public ValueBagTypeDescriptionProvider() : base()
        {
        }

        public ValueBagTypeDescriptionProvider(Type Type) : base(TypeDescriptor.GetProvider(typeof(ValueBag<>).MakeGenericType(Type)))
        {
        }

        public ValueBagTypeDescriptionProvider(TypeDescriptionProvider Parent) : base(Parent)
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type ObjectType, object Instance)
        {
            return new ValueBagTypeDescriptor(base.GetTypeDescriptor(ObjectType, Instance), Instance as IDictionary);
        }
    }

    internal class ValueBagTypeDescriptor : CustomTypeDescriptor
    {
        public ValueBagTypeDescriptor(ICustomTypeDescriptor Descriptor, IDictionary Bag) : base(Descriptor)
        {
            this.Bag = Bag;
            if (this.Bag == null)
            {
                this.Type = typeof(object);
                return;
            }
            if (KsApplication.IsInDesignMode)
            {
                this.Type = typeof(object);
                return;
            }
            var BagType = Bag.GetType();
            if (!BagType.IsGenericType || BagType.GetGenericTypeDefinition() != typeof(ValueBag<>))
            {
            }
            this.Type = BagType.GetGenericArguments()[0];
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var BaseProps = base.GetProperties();
            var R = new PropertyDescriptor[BaseProps.Count + this.Bag?.Count ?? 0];
            var I = 0;
            foreach (PropertyDescriptor P in BaseProps)
            {
                R[I] = P;
                I += 1;
            }
            if (this.Bag == null)
            {
                return new PropertyDescriptorCollection(R);
            }

            foreach (var K in this.Bag.Keys)
            {
                R[I] = new ValueBagPropertyDescriptor((string) K, this.Type);
                I += 1;
            }
            return new PropertyDescriptorCollection(R);
        }

        private readonly IDictionary Bag;
        private readonly Type Type;
    }

    internal class ValueBagPropertyDescriptor : PropertyDescriptor
    {
        public ValueBagPropertyDescriptor(string Name, Type Type) : base(Name, null)
        {
            this.PropName = Name;
            this.PropType = Type;
        }

        public override Type ComponentType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.PropType;
            }
        }

        public override void ResetValue(object component)
        {
            throw new NotSupportedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException();
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return ((IDictionary) component)[this.PropName];
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        private readonly string PropName;
        private readonly Type PropType;
    }
}

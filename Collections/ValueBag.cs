using System.Collections.Generic;
using System.Collections;
using System;
using System.ComponentModel;
using Ks.Common.MVVM;
using Microsoft.VisualBasic.CompilerServices;

namespace Ks
{
    namespace Common
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
                    return ((ICollection<KeyValuePair<string, T>>)this.Dic).IsReadOnly;
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

            private ICollection<string> IDictionary_2_Keys
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

            private ICollection<T> IDictionary_2_Values
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

            private object NonGeneric_Item
            {
                get
                {
                    return this[(string)key];
                }
                set
                {
                    this[(string)key] = (T)value;
                }
            }

            private ICollection IDictionary_Keys
            {
                get
                {
                    return this.Keys;
                }
            }

            private ICollection IDictionary_Values
            {
                get
                {
                    return this.Values;
                }
            }

            private bool IDictionary_IsReadOnly
            {
                get
                {
                    return this.IsReadOnly;
                }
            }

            private bool IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            private int ICollection_Count
            {
                get
                {
                    return this.Count;
                }
            }

            private object SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            private void Add(KeyValuePair<string, T> Item)
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

            private void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<string, T>>)this.Dic).CopyTo(array, arrayIndex);
            }

            private bool Contains(KeyValuePair<string, T> item)
            {
                return ((ICollection<KeyValuePair<string, T>>)this.Dic).Contains(item);
            }

            public bool ContainsKey(string key)
            {
                return this.Dic.ContainsKey(key);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.Dic.GetEnumerator();
            }

            private IEnumerator<KeyValuePair<string, T>> IEnumerable_1_GetEnumerator()
            {
                return this.Dic.GetEnumerator();
            }

            public Dictionary<string, T>.Enumerator GetEnumerator()
            {
                return this.Dic.GetEnumerator();
            }

            private bool Remove(KeyValuePair<string, T> item)
            {
                return ((ICollection<KeyValuePair<string, T>>)this.Dic).Remove(item);
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

            public new string ToString(string format, IFormatProvider formatProvider)
            {
                var R = new System.Text.StringBuilder(Conversions.ToString('{'));
                var Bl = true;
                foreach (var KV in this)
                {
                    if (Bl)
                        Bl = false;
                    else
                        R.Append(", ");

                    R.Append(KV.Key).Append(" : ").Append(string.Format(formatProvider, "{0:" + format + "}", KV.Value));
                }

                return R.Append('}').ToString();
            }

            private bool Contains(object key)
            {
                return this.Contains((string)key);
            }

            private void Add(object key, object value)
            {
                this.Add((string)key, (T)value);
            }

            private void IDictionary_Clear()
            {
                this.Clear();
            }

            private IDictionaryEnumerator IDictionary_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private void Remove(object key)
            {
                this.Remove((string)key);
            }

            private void CopyTo(Array array, int index)
            {
                ((IDictionary)this.Dic).CopyTo(array, index);
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
                var R = new PropertyDescriptor[(BaseProps.Count + this.Bag?.Count ?? 0) - 1 + 1];
                var I = 0;
                foreach (PropertyDescriptor P in BaseProps)
                {
                    R[I] = P;
                    I += 1;
                }
                if (this.Bag == null)
                    return new PropertyDescriptorCollection(R);
                foreach (var K in this.Bag.Keys)
                {
                    R[I] = new ValueBagPropertyDescriptor((string)K, this.Type);
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
                return ((IDictionary)component)[this.PropName];
            }

            public override bool ShouldSerializeValue(object component)
            {
                return true;
            }

            private readonly string PropName;
            private readonly Type PropType;
        }
    }
}

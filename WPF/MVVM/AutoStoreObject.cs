using System.Windows;
using System.Threading.Tasks;
using Mono;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ks
{
    namespace Ks.Common.MVVM
    {
        public abstract class AutoStoreObject : INotifyPropertyChanged
        {
            protected AutoStoreObject()
            {
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual string GetStoreKey(string PropertyName)
            {
                return PropertyName;
            }

            public void Initialize(IDictionary<string, string> Dictionary)
            {
                Verify.True(this._StoreDictionary == null, "Object has already been initialized.");
                this._StoreDictionary = Dictionary;

                this.OnBeforeInitialize();

                foreach (var T in this.GetType().GetBaseTypes())
                {
                    foreach (var KV in MetadataDic[T])
                    {
                        string V = null;
                        if (Dictionary.TryGetValue(this.GetStoreKey(KV.Key), out V))
                            KV.Value.SetCallback.Invoke(this, V);
                    }
                }

                this.OnInitialize();

                this._IsInitialized = true;
            }

            protected virtual void OnInitialize()
            {
            }

            protected virtual void OnBeforeInitialize()
            {
            }

            protected bool SetProperty<T>(ref T Source, T Value, [CallerMemberName()] string PropertyName = null)
            {
                Verify.True(this.IsInitialized, "Object is not initialized.");

                if (!object.Equals(Source, Value))
                {
                    Source = Value;
                    this.NotifyPropertyChanged(PropertyName);

                    foreach (var T in this.GetType().GetBaseTypes())
                    {
                        PropertyMetadata M = default(PropertyMetadata);
                        if (MetadataDic[T].TryGetValue(PropertyName, ref M))
                        {
                            var Str = M.ToStringCallback.Invoke(Source);
                            StoreDictionary[this.GetStoreKey(PropertyName)] = Str;
                            break;
                        }
                    }

                    return true;
                }

                return false;
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs E)
            {
                PropertyChanged?.Invoke(this, E);
            }

            protected void NotifyPropertyChanged([CallerMemberName()] string PropertyName = null)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(PropertyName));
            }

            protected static object RegisterProperty(Type OwnerType, string Name, Action<AutoStoreObject, string> SetCallback = null, Func<object, string> ToStringCallBack = null)
            {
                foreach (var T in OwnerType.GetBaseTypes())
                    Verify.False(MetadataDic[T].ContainsKey(Name), "Name already eists.");

                if (SetCallback == null)
                {
                    SetCallback = (M, O) =>
                    {
                        M.GetType().GetProperty(Name).SetValue(M, O); // ToDo Invoke CType on O.
                    };
                }
                if (ToStringCallBack == null)
                    ToStringCallBack = O => string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", O);
                MetadataDic[OwnerType].Add(Name, new PropertyMetadata(SetCallback, ToStringCallBack));
                return null;
            }

            private bool _IsInitialized;

            public bool IsInitialized
            {
                get
                {
                    return this._IsInitialized;
                }
            }

            private IDictionary<string, string> _StoreDictionary;

            protected IDictionary<string, string> StoreDictionary
            {
                get
                {
                    return this._StoreDictionary;
                }
            }

            private static readonly CreateInstanceDictionary<Type, Dictionary<string, PropertyMetadata>> MetadataDic = CreateInstanceDictionary.Create<Type, Dictionary<string, PropertyMetadata>>();

            private struct PropertyMetadata
            {
                public PropertyMetadata(Action<AutoStoreObject, string> SetCallback, Func<object, string> ToStringCallback)
                {
                    this._SetCallback = SetCallback;
                    this._ToStringCallback = ToStringCallback;
                }

                private readonly Action<AutoStoreObject, string> _SetCallback;

                public Action<AutoStoreObject, string> SetCallback
                {
                    get
                    {
                        return this._SetCallback;
                    }
                }

                private readonly Func<object, string> _ToStringCallback;

                public Func<object, string> ToStringCallback
                {
                    get
                    {
                        return this._ToStringCallback;
                    }
                }
            }
        }
    }
}

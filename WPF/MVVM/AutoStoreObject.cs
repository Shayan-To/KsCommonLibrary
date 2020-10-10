using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

using Mono;

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
            Verify.True(this.StoreDictionary == null, "Object has already been initialized.");
            this.StoreDictionary = Dictionary;

            this.OnBeforeInitialize();

            foreach (var T in this.GetType().GetBaseTypes())
            {
                foreach (var KV in MetadataDic[T])
                {
                    if (Dictionary.TryGetValue(this.GetStoreKey(KV.Key), out var V))
                    {
                        KV.Value.SetCallback.Invoke(this, V);
                    }
                }
            }

            this.OnInitialize();

            this.IsInitialized = true;
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

                foreach (var t in this.GetType().GetBaseTypes())
                {
                    if (MetadataDic[t].TryGetValue(PropertyName, out var M))
                    {
                        var Str = M.ToStringCallback.Invoke(Source);
                        this.StoreDictionary[this.GetStoreKey(PropertyName)] = Str;
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
            {
                Verify.False(MetadataDic[T].ContainsKey(Name), "Name already eists.");
            }

            if (SetCallback == null)
            {
                SetCallback = (M, O) =>
                {
                    M.GetType().GetProperty(Name).SetValue(M, O); // ToDo Invoke CType on O.
                };
            }
            if (ToStringCallBack == null)
            {
                ToStringCallBack = O => string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", O);
            }

            MetadataDic[OwnerType].Add(Name, new PropertyMetadata(SetCallback, ToStringCallBack));
            return null;
        }

        public bool IsInitialized { get; private set; }

        protected IDictionary<string, string> StoreDictionary { get; private set; }

        private static readonly CreateInstanceDictionary<Type, Dictionary<string, PropertyMetadata>> MetadataDic = CreateInstanceDictionary.Create<Type, Dictionary<string, PropertyMetadata>>();

        private struct PropertyMetadata
        {
            public PropertyMetadata(Action<AutoStoreObject, string> SetCallback, Func<object, string> ToStringCallback)
            {
                this.SetCallback = SetCallback;
                this.ToStringCallback = ToStringCallback;
            }

            public Action<AutoStoreObject, string> SetCallback { get; }

            public Func<object, string> ToStringCallback { get; }
        }
    }
}

using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks
{
    namespace Common
    {
        public class AutoStoreDictionary : BaseDictionary<string, string>, IFormattable, IDisposable, IOrderedDictionary<string, string>
        {
            public AutoStoreDictionary(string Path) : this(System.IO.File.Open(Path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read))
            {
            }

            public AutoStoreDictionary(System.IO.Stream Stream, bool LeaveOpen = false)
            {
                Verify.True(Stream.CanRead & Stream.CanWrite & Stream.CanSeek, "Stream must have read, write and seek capabilities.");

                this.TaskDelayer = new TaskDelayer(this.Store, TimeSpan.FromSeconds(10));

                this.Stream = Stream;
                this._LeaveOpen = LeaveOpen;

                if (Stream.Length == 0)
                {
                    this.BaseDic = new ConcurrentOrderedDictionary<string, string>();
                    return;
                }

                var BinaryData = Stream.ReadToEnd();
                var Data = new string(System.Text.Encoding.UTF8.GetChars(BinaryData));

                // For backward compatibility.
                OrderedDictionary<string, string> T;
                if (Data[0] == '{')
                    T = Utilities.Serialization.DicFromString(Data);
                else
                    T = Utilities.Serialization.DicFromStringMultiline(Data);

                this.BaseDic = new ConcurrentOrderedDictionary<string, string>(T, StringComparer.InvariantCulture);
            }

            private void Collection_Changed()
            {
                this.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed);
            }

            private void Store()
            {
                string SerializedData;

                SerializedData = Utilities.Serialization.DicToStringMultiline(this.BaseDic);

                var SerializedBinaryData = System.Text.Encoding.UTF8.GetBytes(SerializedData);
                this.Stream.Position = 0;
                this.Stream.Write(SerializedBinaryData, 0, SerializedBinaryData.Length);
                this.Stream.SetLength(SerializedBinaryData.Length);
                this.Stream.Flush();
            }

            public override int Count
            {
                get
                {
                    return this.BaseDic.Count;
                }
            }

            public override string this[string key]
            {
                get
                {
                    return this.BaseDic[key];
                }
                set
                {
                    this.BaseDic[key] = value;
                    this.Collection_Changed();
                }
            }

            public override ICollection<string> Keys
            {
                get
                {
                    return (ICollection<string>)this.KeysList;
                }
            }

            public override ICollection<string> Values
            {
                get
                {
                    return (ICollection<string>)this.ValuesList;
                }
            }

            public IReadOnlyList<string> KeysList
            {
                get
                {
                    return this.BaseDic.KeysList;
                }
            }

            public IReadOnlyList<string> ValuesList
            {
                get
                {
                    return this.BaseDic.ValuesList;
                }
            }

            public KeyValuePair<string, string> this[int index]
            {
                get
                {
                    return this.BaseDic[index];
                }
                set
                {
                    this.BaseDic[index] = value;
                    this.Collection_Changed();
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            public override void Add(string key, string value)
            {
                this.BaseDic.Add(key, value);
                this.Collection_Changed();
            }

            public override void Clear()
            {
                this.BaseDic.Clear();
                this.Collection_Changed();
            }

            public void Insert(int index, string key, string value)
            {
                this.BaseDic.Insert(index, key, value);
                this.Collection_Changed();
            }

            public void RemoveAt(int index)
            {
                this.BaseDic.RemoveAt(index);
                this.Collection_Changed();
            }

            public override bool ContainsKey(string key)
            {
                return this.BaseDic.ContainsKey(key);
            }

            public int IndexOf(string key)
            {
                return this.BaseDic.IndexOf(key);
            }

            public override bool Remove(string key)
            {
                var res = this.BaseDic.Remove(key);
                this.Collection_Changed();
                return res;
            }

            public override bool TryGetValue(string key, out string value)
            {
                return this.BaseDic.TryGetValue(key, out value);
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                return this.BaseDic.GetEnumerator();
            }

            IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
            {
                return this.GetDictionaryEnumerator();
            }

            protected override IEnumerator<KeyValuePair<string, string>> _GetEnumerator()
            {
                return this.GetEnumerator();
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
                        Bl = false;
                    else
                        R.Append(", ");

                    R.Append(KV.Key).Append(" : ").Append(string.Format(formatProvider, "{0:" + format + "}", KV.Value));
                }

                return R.Append('}').ToString();
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (KeyValuePair<string, string>)value;
                }
            }

            object IOrderedDictionary.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (KeyValuePair<string, string>)value;
                }
            }

            int IList.Add(object value)
            {
                this.ICollection_Add((KeyValuePair<string, string>)value);
                return this.Count - 1;
            }

            void IList<KeyValuePair<string, string>>.Insert(int index, KeyValuePair<string, string> item)
            {
                this.Insert(index, item.Key, item.Value);
            }

            void IList.Insert(int index, object value)
            {
                var kv = (KeyValuePair<string, string>)value;
                this.Insert(index, kv.Key, kv.Value);
            }

            void IOrderedDictionary.Insert(int index, object key, object value)
            {
                this.Insert(index, (string)key, (string)value);
            }

            void IList.Remove(object value)
            {
                this.ICollection_Remove((KeyValuePair<string, string>)value);
            }

            private int IList_IndexOf(KeyValuePair<string, string> item)
            {
                var R = this.IndexOf(item.Key);
                var T = this[R].Value;

                if (R == -1)
                    return -1;
                if (!object.Equals(item.Value, T))
                    return -1;
                return R;
            }

            int IList<KeyValuePair<string, string>>.IndexOf(KeyValuePair<string, string> item)
            {
                return this.IList_IndexOf(item);
            }

            int IList.IndexOf(object value)
            {
                return this.IList_IndexOf((KeyValuePair<string, string>)value);
            }

            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                return this.GetDictionaryEnumerator();
            }

            bool IList.Contains(object value)
            {
                return this.ICollection_Contains((KeyValuePair<string, string>)value);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this._IsDisposed)
                {
                    this._IsDisposed = true;

                    // This is more critical to be given over to the non-reliable GC.
                    this.TaskDelayer.Dispose();

                    if (disposing)
                    {
                        // Dispose managed state (managed objects).
                        if (!this.LeaveOpen)
                            this.Stream.Dispose();
                    }

                    // Set large fields to null.
                    this.BaseDic = null;
                }
            }

            public void Dispose()
            {
                this.Dispose(true);
            }

            private bool _IsDisposed;

            public bool IsDisposed
            {
                get
                {
                    return this._IsDisposed;
                }
            }

            private readonly bool _LeaveOpen;

            public bool LeaveOpen
            {
                get
                {
                    return this._LeaveOpen;
                }
            }

            private ConcurrentOrderedDictionary<string, string> BaseDic;
            private readonly System.IO.Stream Stream;
            private readonly TaskDelayer TaskDelayer;
        }
    }
}

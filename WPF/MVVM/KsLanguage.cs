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
using Microsoft.VisualBasic.CompilerServices;

namespace Ks
{
    namespace Common.MVVM
    {
        public class KsLanguage : IDisposable
        {
            public KsLanguage(System.IO.Stream Stream)
            {
                this.TaskDelayer = new TaskDelayer(this.DoStore, TimeSpan.FromSeconds((double)10));

                this.Stream = Stream;

                Stream.Position = 0;
                this.Csv = CsvData.Parse(new string(System.Text.Encoding.UTF8.GetChars(Stream.ReadToEnd())), false);

                Verify.True(this.Csv.Columns.Count <= 2, "Invalid language file.");
                while (this.Csv.Columns.Count < 2)
                    this.Csv.Columns.Add();

                var HeaderSet = new HashSet<string>();
                var I = 0;
                while (I < this.Csv.Entries.Count)
                {
                    var E = this.Csv.Entries[I];
                    var Key = E[0].ToLower();
                    var Value = E[1];

                    if ((Key.Length == 0) & (Value.Length == 0))
                        break;

                    Verify.True(HeaderSet.Add(Key), "Duplicate language property.");

                    switch (Key)
                    {
                        case var @case when @case == nameof(this.Id).ToLower():
                            {
                                this._Id = Value;
                                break;
                            }

                        case var case1 when case1 == nameof(this.Name).ToLower():
                            {
                                this._Name = Value;
                                break;
                            }

                        case var case2 when case2 == nameof(this.NativeName).ToLower():
                            {
                                this._NativeName = Value;
                                break;
                            }

                        case var case3 when case3 == nameof(this.Direction).ToLower():
                            {
                                Value = Value.ToLower();
                                Verify.True((Operators.CompareString(Value, nameof(FlowDirection.LeftToRight).ToLower(), TextCompare: false) == 0) | (Operators.CompareString(Value, nameof(FlowDirection.RightToLeft).ToLower(), TextCompare: false) == 0) | (Value == "rtl") | (Value == "ltr"));
                                this._Direction = ((Value == "ltr") | (Operators.CompareString(Value, nameof(FlowDirection.LeftToRight).ToLower(), TextCompare: false) == 0)) ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
                                break;
                            }

                        default:
                            {
                                Verify.Fail("Invalid language property.");
                                break;
                            }
                    }

                    I += 1;
                }

                if (this.Id == null)
                    this._Id = "";
                if (this.Name == null & this.NativeName == null)
                    this._Name = "Default";
                if (this.NativeName == null)
                    this._NativeName = "";

                if (I == this.Csv.Entries.Count)
                {
                    var E = this.Csv.Entries.LastOrDefault();
                    if (E == null || !((E[0].Length == 0) & (E[1].Length == 0)))
                        this.Csv.Entries.Add();
                }

                I += 1;
                while (I < this.Csv.Entries.Count)
                {
                    var E = this.Csv.Entries[I];
                    var Key = E[0];
                    var Value = E[1];

                    if (Key.Length == 0)
                        Verify.True(Value.Length == 0, "Cannot set a translation to an empty string.");
                    else
                        this.Dictionary.Add(Key, Value);

                    I += 1;
                }

                this.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed);
            }

            public string GetTranslation(string Text)
            {
                if (Text == null || Text.Length == 0)
                    return Text;

                string R = null;

                if (this.Dictionary.TryGetValue(Text, out R))
                    return R;

                this.Dictionary.Add(Text, Text);

                lock (this.LockObject)
                {
                    var E = this.Csv.Entries.Add();
                    E[0] = Text;
                    E[1] = Text;
                }

                this.TaskDelayer.RunTask(TaskDelayerRunningMode.Delayed);

                return Text;
            }

            private void DoStore()
            {
                string Str;
                lock (this.LockObject)
                {
                    var HeaderSize = 0;
                    while (this.Csv.Entries.Count != HeaderSize)
                    {
                        var H = this.Csv.Entries[HeaderSize];

                        if ((H[0].Length == 0) & (H[1].Length == 0))
                            break;
                        HeaderSize += 1;
                    }

                    Assert.True(HeaderSize <= 4);
                    while (HeaderSize < 4)
                    {
                        this.Csv.Entries.Insert(0);
                        HeaderSize += 1;
                    }

                    var E = this.Csv.Entries[0];
                    E[0] = nameof(this.Id);
                    E[1] = this.Id;

                    E = this.Csv.Entries[1];
                    E[0] = nameof(this.Name);
                    E[1] = this.Name;

                    E = this.Csv.Entries[2];
                    E[0] = nameof(this.NativeName);
                    E[1] = this.NativeName;

                    E = this.Csv.Entries[3];
                    E[0] = nameof(this.Direction);
                    E[1] = ((int)this.Direction == (int)FlowDirection.LeftToRight) ? nameof(FlowDirection.LeftToRight) : nameof(FlowDirection.RightToLeft);

                    Str = this.Csv.ToString();
                }

                var Bytes = System.Text.Encoding.UTF8.GetBytes(Str);
                this.Stream.Position = 0;
                this.Stream.Write(Bytes, 0, Bytes.Length);
                this.Stream.SetLength((long)Bytes.Length);
                this.Stream.Flush();
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
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private readonly string _Id;

            public string Id
            {
                get
                {
                    return this._Id;
                }
            }

            private readonly string _Name;

            public string Name
            {
                get
                {
                    return this._Name;
                }
            }

            private readonly string _NativeName;

            public string NativeName
            {
                get
                {
                    return this._NativeName;
                }
            }

            private readonly FlowDirection _Direction;

            public FlowDirection Direction
            {
                get
                {
                    return this._Direction;
                }
            }

            private bool _IsDisposed;

            public bool IsDisposed
            {
                get
                {
                    return this._IsDisposed;
                }
            }

            private readonly object LockObject = new object();

            private readonly Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            private readonly CsvData Csv;
            private readonly TaskDelayer TaskDelayer;
            private readonly System.IO.Stream Stream;
        }
    }
}

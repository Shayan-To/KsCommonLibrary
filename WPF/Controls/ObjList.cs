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

namespace Ks
{
    namespace Common.Controls
    {
        public class ObjList : BaseList<Obj>, System.Windows.Markup.IAddChild
        {
            public ObjList(TextBlock Parent)
            {
                this._Parent = Parent;
            }

            private void GotIn(Obj Obj)
            {
                Obj.ReportParent(this.Parent);
                this.Parent.ReportObjGotIn(Obj);
            }

            private void WentOut(Obj Obj)
            {
                this.Parent.ReportObjWentOut(Obj);
            }

            public override int Count
            {
                get
                {
                    return this.List.Count;
                }
            }

            public override Obj this[int index]
            {
                get
                {
                    return this.List[index];
                }
                set
                {
                    this.WentOut(this.List[index]);
                    this.List[index] = value;
                    this.GotIn(value);
                    this.Parent.ReportObjChanged();
                }
            }

            public override void Clear()
            {
                foreach (var O in this.List)
                    this.WentOut(O);
                this.List.Clear();
                this.Parent.ReportObjChanged();
            }

            public override void Insert(int index, Obj item)
            {
                this.List.Insert(index, item);
                this.GotIn(item);
                this.Parent.ReportObjChanged();
            }

            public override void RemoveAt(int index)
            {
                this.WentOut(this.List[index]);
                this.List.RemoveAt(index);
                this.Parent.ReportObjChanged();
            }

            protected override IEnumerator<Obj> _GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            public List<Obj>.Enumerator GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            public void AddChild(object value)
            {
                this.Add((Obj)value);
            }

            public void AddText(string text)
            {
                throw new NotSupportedException();
            }

            private readonly TextBlock _Parent;

            public TextBlock Parent
            {
                get
                {
                    return this._Parent;
                }
            }

            private readonly List<Obj> List = new List<Obj>();
        }
    }
}

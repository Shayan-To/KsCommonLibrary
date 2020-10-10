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

using Mono;

namespace Ks.Common.Controls
{
    public class ObjList : BaseList<Obj>, System.Windows.Markup.IAddChild
    {
        public ObjList(TextBlock Parent)
        {
            this.Parent = Parent;
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
            get => this.List[index];
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
            {
                this.WentOut(O);
            }

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
            this.Add((Obj) value);
        }

        public void AddText(string text)
        {
            throw new NotSupportedException();
        }

        public TextBlock Parent { get; }

        private readonly List<Obj> List = new List<Obj>();
    }
}

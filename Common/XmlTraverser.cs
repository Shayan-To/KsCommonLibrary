﻿using System;

namespace Ks
{
    namespace Common
    {
        public abstract class XmlTraverser
        {
            public static DelegateXmlTraverser Create(Action<System.Xml.XmlNode, VisitAction> VisitDelegate)
            {
                return new DelegateXmlTraverser(VisitDelegate);
            }

            protected abstract void Visit(System.Xml.XmlNode Node, VisitAction VisitAction);

            public void Traverse(System.Xml.XmlNode Node)
            {
                this._VisitAction.Reset();
                this.Visit(Node, this._VisitAction);

                if (this._VisitAction.TraverseChildren)
                {
                    foreach (System.Xml.XmlNode Child in Node.ChildNodes)
                        this.Traverse(Child);
                }
            }

            private readonly VisitAction _VisitAction = new VisitAction();

            public class VisitAction
            {
                public VisitAction()
                {
                    this.Reset();
                }

                public void Reset()
                {
                    this.TraverseChildren = true;
                }

                private bool _TraverseChildren;

                public bool TraverseChildren
                {
                    get
                    {
                        return this._TraverseChildren;
                    }
                    set
                    {
                        this._TraverseChildren = value;
                    }
                }
            }
        }

        public class DelegateXmlTraverser : XmlTraverser
        {
            public DelegateXmlTraverser(Action<System.Xml.XmlNode, VisitAction> VisitDelegate)
            {
                this.VisitDelegate = VisitDelegate;
            }

            protected override void Visit(System.Xml.XmlNode Node, VisitAction VisitAction)
            {
                this.VisitDelegate.Invoke(Node, VisitAction);
            }

            private readonly Action<System.Xml.XmlNode, VisitAction> VisitDelegate;
        }
    }
}

using System;
using System.Xml.Linq;

namespace Ks.Common
{
    public abstract class XmlLinqTraverser
    {
        public static DelegateXmlLinqTraverser Create(Action<XElement, VisitAction> VisitDelegate)
        {
            return new DelegateXmlLinqTraverser(VisitDelegate);
        }

        protected abstract void Visit(XElement Node, VisitAction VisitAction);

        public XElement Traverse(XElement Node)
        {
            var NodeReplaced = false;

            this._VisitAction.Reset(Node);
            this.Visit(Node, this._VisitAction);
            if (this._VisitAction.ReplaceWithElement != Node)
            {
                Node.ReplaceWith(this._VisitAction.ReplaceWithElement);
                Node = this._VisitAction.ReplaceWithElement;
                NodeReplaced = true;
            }

            if (this._VisitAction.TraverseChildren)
            {
                XElement ChangedChild = null;

                foreach (var Child in Node.Elements())
                {
                    ChangedChild = this.Traverse(Child);
                    if (ChangedChild != null)
                        break;
                }

                while (ChangedChild != null)
                {
                    var T = ChangedChild;
                    ChangedChild = null;
                    foreach (var Child in T.ElementsAfterSelf())
                    {
                        ChangedChild = this.Traverse(Child);
                        if (ChangedChild != null)
                            break;
                    }
                }
            }

            if (NodeReplaced)
                return Node;

            return null;
        }

        private readonly VisitAction _VisitAction = new VisitAction();

        public class VisitAction
        {
            public VisitAction()
            {
                this.Reset(null);
            }

            public void Reset(XElement Element)
            {
                this.TraverseChildren = true;
                this.ReplaceWithElement = Element;
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

            private XElement _ReplaceWithElement;

            public XElement ReplaceWithElement
            {
                get
                {
                    return this._ReplaceWithElement;
                }
                set
                {
                    this._ReplaceWithElement = value;
                }
            }
        }
    }

    public class DelegateXmlLinqTraverser : XmlLinqTraverser
    {
        public DelegateXmlLinqTraverser(Action<XElement, VisitAction> VisitDelegate)
        {
            this.VisitDelegate = VisitDelegate;
        }

        protected override void Visit(XElement Node, VisitAction VisitAction)
        {
            this.VisitDelegate.Invoke(Node, VisitAction);
        }

        private readonly Action<XElement, VisitAction> VisitDelegate;
    }
}

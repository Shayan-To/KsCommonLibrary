using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Ks.Common
{
    public abstract class Treeer<T, TNode> where T : class
    {

        public Treeer()
        {
            this.Updated = this._UpdatedSubject.AsObservable();
        }

        public void Update()
        {
#if false
            var prevNodes = this.Nodes;
            this.Nodes = new(prevNodes.Count, this.Nodes.Comparer);

            TNode Process(T? t)
            {
                TNode? node;

                if (t == null)
                {
                    node = this._Root;
                }
                else
                {
                    prevNodes.TryGetValue(t, out node);
                }

                if (node == null)
                {
                    node = this.CreateNode(t);
                }
                this.SetNodeFor(t, node);


                var children = this.GetChildren(t);
                var nodes = children.Select(Process).ToArray();
                this.SetChildren(node, nodes);

                return node;
            }

            var nodes = prevNodes.Values.AsEnumerable();
            if (this._Root != null)
            {
                nodes = nodes.Append(this._Root);
            }
            foreach (var node in nodes)
            {
                this.SetChildren(node, Utilities.Typed<TNode>.EmptyArray);
            }

            Process(null);
#endif
            var prevNodes = this.Nodes;
            this.Nodes = new(this.Nodes.Count, this.Nodes.Comparer);

            TNode Process(T? t)
            {
                TNode? node;

                if (t == null)
                {
                    node = this._Root ?? this.CreateNode(t, default);
                }
                else
                {
                    prevNodes.TryGetValue(t, out var prevNode);
                    node = this.CreateNode(t, prevNode);
                }
                this.SetNodeFor(t, node);

                var children = this.GetChildren(t);
                var nodes = children.Select(Process).ToArray();
                this.SetChildren(node, nodes);

                return node;
            }

            Process(null);

            this._UpdatedSubject.OnNext(default);
        }

        public TNode? GetNodeFor(T? t)
        {
            if (t == null)
            {
                return this._Root;
            }
            this.Nodes.TryGetValue(t, out var node);
            return node;
        }

        private void SetNodeFor(T? t, TNode node)
        {
            if (t == null)
            {
                this._Root = node;
            }
            else
            {
                this.Nodes[t] = node;
            }
        }

        protected abstract IEnumerable<T> GetChildren(T? t);
        protected abstract void SetChildren(TNode node, IEnumerable<TNode> children);
        protected abstract TNode CreateNode(T? t, TNode? prevNode);

        private TNode? _Root;
        public TNode Root
        {
            get
            {
                if (this._Root == null)
                {
                    this.Update();
                }
                return this._Root!;
            }
        }

        private readonly Subject<Unit> _UpdatedSubject = new();
        public IObservable<Unit> Updated{ get; }

        protected Dictionary<T, TNode> Nodes { get; private set; } = new(ReferenceEqualityComparer<T>.Default);

    }
}

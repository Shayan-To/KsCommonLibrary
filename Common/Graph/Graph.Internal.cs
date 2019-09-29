using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    partial class Graph
    {
        public Graph(IEnumerable<GraphConstraint.NodeAddition> nodeAdditionConstraints,
                     IEnumerable<GraphConstraint.EdgeAddition> edgeAdditionConstraints,
                     IEnumerable<GraphConstraint.NodeRemoval> nodeRemovalConstraints,
                     IEnumerable<GraphConstraint.EdgeRemoval> edgeRemovalConstraints)
        {
            this.Operations = new GraphInternalOperations(this);

            this.NodeAdditionConstraints = nodeAdditionConstraints.ToArray().AsReadOnly();
            this.EdgeAdditionConstraints = edgeAdditionConstraints.ToArray().AsReadOnly();
            this.NodeRemovalConstraints = nodeRemovalConstraints.ToArray().AsReadOnly();
            this.EdgeRemovalConstraints = edgeRemovalConstraints.ToArray().AsReadOnly();

            var allConstraints = new GraphConstraint[0]
                        .Concat(this.NodeAdditionConstraints)
                        .Concat(this.EdgeAdditionConstraints)
                        .Concat(this.NodeRemovalConstraints)
                        .Concat(this.EdgeRemovalConstraints);
            foreach (var c in allConstraints)
            {
                c.GraphOps = this.Operations;
            }
        }

        private T ProcessConstraints<T, TConstraint>(IReadOnlyList<TConstraint> constraints, Func<TConstraint, T?> call) where T : struct
        {
            if (constraints.Count == 0)
            {
                throw new NotSupportedException($"This method is not supported in this graph instance.");
            }

            foreach (var c in constraints.SkipLast(1))
            {
                Verify.False(call(c).HasValue);
            }

            this.Operations.OperationAllowed = true;
            try
            {
                var c = constraints[constraints.Count - 1];
                return call(c).Value;
            }
            finally
            {
                this.Operations.OperationAllowed = false;
            }
        }

        private GraphNode AddNodeInternal(GraphNodeData data)
        {
            var edgesFrom = new SortedList<GraphEdge>((a, b) =>
            {
                var c = a.To.Id - b.To.Id;
                if (c != 0)
                {
                    return c;
                }
                return a.Id - b.Id;
            });
            var edgesTo = new SortedList<GraphEdge>((a, b) =>
            {
                var c = a.From.Id - b.From.Id;
                if (c != 0)
                {
                    return c;
                }
                return a.Id - b.Id;
            });
            var node = new InternalNode()
            {
                Node = new GraphNode(this.Nodes.Count, data, edgesFrom.AsReadOnly(), edgesTo.AsReadOnly()),
                EdgesFrom = edgesFrom,
                EdgesTo = edgesTo
            };
            this.Nodes.Add(node);
            return node.Node;
        }

        private void RemoveNodeInternal(GraphNode node)
        {
            Verify.True(node.EdgesFrom.Count == 0 & node.EdgesTo.Count == 0, "Cannot remove a node with edges connected to it.");
            this.Nodes.RemoveAt(node.Id);
            for (var i = node.Id; i < this.Nodes.Count; i++)
            {
                var ni = this.Nodes[i];
                var n = ni.Node;
                ni.Node = new GraphNode(i, n.Data, n.EdgesFrom, n.EdgesTo);
                this.Nodes[i] = ni;
            }
        }

        private GraphEdge AddEdgeInternal(GraphNode from, GraphNode to, GraphEdgeData data)
        {
            var edge = new GraphEdge(this.EdgeIdCounter, from, to, data);
            this.EdgeIdCounter += 1;
            this.Nodes[from.Id].EdgesFrom.Add(edge);
            this.Nodes[to.Id].EdgesTo.Add(edge);
            return edge;
        }

        private void RemoveEdgeInternal(GraphEdge edge)
        {
            this.Nodes[edge.From.Id].EdgesFrom.Remove(edge);
            this.Nodes[edge.To.Id].EdgesTo.Remove(edge);
        }

        public IReadOnlyList<GraphConstraint.NodeAddition> NodeAdditionConstraints { get; }
        public IReadOnlyList<GraphConstraint.EdgeAddition> EdgeAdditionConstraints { get; }
        public IReadOnlyList<GraphConstraint.NodeRemoval> NodeRemovalConstraints { get; }
        public IReadOnlyList<GraphConstraint.EdgeRemoval> EdgeRemovalConstraints { get; }

        private int EdgeIdCounter = 0;
        private readonly GraphInternalOperations Operations;

        public class GraphInternalOperations
        {
            public GraphInternalOperations(Graph parent)
            {
                this.Parent = parent;
            }

            private void VerifyPermission()
            {
                Verify.True(this.OperationAllowed);
            }

            public GraphNode AddNodeInternal(GraphNodeData data)
            {
                this.VerifyPermission();
                return this.Parent.AddNodeInternal(data);
            }

            public GraphEdge AddEdgeInternal(GraphNode from, GraphNode to, GraphEdgeData data)
            {
                this.VerifyPermission();
                return this.Parent.AddEdgeInternal(from, to, data);
            }

            public void RemoveNodeInternal(GraphNode node)
            {
                this.VerifyPermission();
                this.Parent.RemoveNodeInternal(node);
            }

            public void RemoveEdgeInternal(GraphEdge edge)
            {
                this.VerifyPermission();
                this.Parent.RemoveEdgeInternal(edge);
            }

            public Graph Parent { get; }
            public bool OperationAllowed { get; internal set; }
        }
    }
}

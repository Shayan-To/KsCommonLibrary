namespace Ks.Common
{
    public abstract class GraphConstraint
    {
        private Graph.GraphInternalOperations _GraphOps;

        public Graph.GraphInternalOperations GraphOps
        {
            get => this._GraphOps;
            internal set
            {
                Verify.True(this._GraphOps == null, $"Cannot set {nameof(GraphConstraint)}.{nameof(this.GraphOps)} twice.");
                this._GraphOps = value;
            }
        }

        public Graph Graph => this.GraphOps.Parent;

        public abstract class NodeAddition : GraphConstraint
        {
            public abstract GraphNode? OnAddingNode(GraphNodeData data);
        }

        public abstract class EdgeAddition : GraphConstraint
        {
            public abstract GraphEdge? OnAddingEdge(GraphNode from, GraphNode to, GraphEdgeData data);
        }

        public abstract class NodeRemoval : GraphConstraint
        {
            public abstract OperationDone? OnRemovingNode(GraphNode node);
        }

        public abstract class EdgeRemoval : GraphConstraint
        {
            public abstract OperationDone? OnRemovingEdge(GraphEdge edge);
        }

        public struct OperationDone
        {
        }
    }
}

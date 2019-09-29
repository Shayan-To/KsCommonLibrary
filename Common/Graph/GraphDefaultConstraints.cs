namespace Ks.Common
{
    public static class GraphDefaultConstraints
    {
        public static GraphBuilder AddDefaultNodeAdditionConstraint(this GraphBuilder builder)
        {
            return builder.AddNodeAdditionConstraint(new NodeAddition());
        }

        public static GraphBuilder AddDefaultEdgeAdditionConstraint(this GraphBuilder builder)
        {
            return builder.AddEdgeAdditionConstraint(new EdgeAddition());
        }

        public static GraphBuilder AddDefaultNodeRemovalConstraint(this GraphBuilder builder)
        {
            return builder.AddNodeRemovalConstraint(new NodeRemoval());
        }

        public static GraphBuilder AddDefaultEdgeRemovalConstraint(this GraphBuilder builder)
        {
            return builder.AddEdgeRemovalConstraint(new EdgeRemoval());
        }

        public static GraphBuilder AddDefaultConstraints(this GraphBuilder builder)
        {
            return builder
                .AddDefaultNodeAdditionConstraint()
                .AddDefaultEdgeAdditionConstraint()
                .AddDefaultNodeRemovalConstraint()
                .AddDefaultEdgeRemovalConstraint();
        }

        public class NodeAddition : GraphConstraint.NodeAddition
        {
            public override GraphNode? OnAddingNode(GraphNodeData data)
            {
                return this.GraphOps.AddNodeInternal(data);
            }
        }

        public class EdgeAddition : GraphConstraint.EdgeAddition
        {
            public override GraphEdge? OnAddingEdge(GraphNode from, GraphNode to, GraphEdgeData data)
            {
                return this.GraphOps.AddEdgeInternal(from, to, data);
            }
        }

        public class NodeRemoval : GraphConstraint.NodeRemoval
        {
            public override OperationDone? OnRemovingNode(GraphNode node)
            {
                this.GraphOps.RemoveNodeInternal(node);
                return new OperationDone();
            }
        }

        public class EdgeRemoval : GraphConstraint.EdgeRemoval
        {
            public override OperationDone? OnRemovingEdge(GraphEdge edge)
            {
                this.GraphOps.RemoveEdgeInternal(edge);
                return new OperationDone();
            }
        }
    }
}

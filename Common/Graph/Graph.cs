using System.Collections.Generic;

namespace Ks.Common
{
    public partial class Graph
    {
        public static GraphBuilder Builder()
        {
            return new GraphBuilder();
        }

        public GraphNode AddNode(GraphNodeData data = null)
        {
            return this.ProcessConstraints(this.NodeAdditionConstraints, c => c.OnAddingNode(data));
        }

        public GraphEdge AddEdge(GraphNode from, GraphNode to, GraphEdgeData data = null)
        {
            return this.ProcessConstraints(this.EdgeAdditionConstraints, c => c.OnAddingEdge(from, to, data));
        }

        public void RemoveNode(GraphNode node)
        {
            this.ProcessConstraints(this.NodeRemovalConstraints, c => c.OnRemovingNode(node));
        }

        public void RemoveEdge(GraphEdge edge)
        {
            this.ProcessConstraints(this.EdgeRemovalConstraints, c => c.OnRemovingEdge(edge));
        }

        public GraphNode GetNode(int id)
        {
            return this.Nodes[id].Node;
        }

        private readonly List<InternalNode> Nodes = new List<InternalNode>();

        private struct InternalNode
        {
            public GraphNode Node { get; set; }
            public SortedList<GraphEdge> EdgesFrom { get; set; }
            public SortedList<GraphEdge> EdgesTo { get; set; }
        }
    }
}

using System.Collections.Generic;

namespace Ks.Common
{
    public struct GraphNode
    {
        internal GraphNode(int id, GraphNodeData data, IReadOnlyList<GraphEdge> edgesFrom, IReadOnlyList<GraphEdge> edgesTo)
        {
            this.Id = id;
            this.EdgesFrom = edgesFrom;
            this.EdgesTo = edgesTo;
            this.Data = data;
        }

        public IReadOnlyList<GraphEdge> GetEdgesFrom(GraphNode from)
        {
            var (index, count) = this.EdgesTo.BinarySearch(from, (e, n) => e.From.Id - n.Id);
            return this.EdgesTo.SubList(index, count);
        }

        public IReadOnlyList<GraphEdge> GetEdgesTo(GraphNode to)
        {
            var (index, count) = this.EdgesFrom.BinarySearch(to, (e, n) => e.To.Id - n.Id);
            return this.EdgesFrom.SubList(index, count);
        }

        public int Id { get; }
        public IReadOnlyList<GraphEdge> EdgesFrom { get; }
        public IReadOnlyList<GraphEdge> EdgesTo { get; }
        public GraphNodeData Data { get; }
    }
}

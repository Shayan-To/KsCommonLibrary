namespace Ks.Common
{
    public struct GraphEdge
    {

        internal GraphEdge(int id, GraphNode from, GraphNode to, GraphEdgeData data)
        {
            this.Id = id;
            this.From = from;
            this.To = to;
            this.Data = data;
        }

        public int Id { get; }
        public GraphNode From { get; }
        public GraphNode To { get; }
        public GraphEdgeData Data { get; }
    }
}

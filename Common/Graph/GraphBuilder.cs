using System.Collections.Generic;

namespace Ks.Common
{
    public class GraphBuilder
    {
        public GraphBuilder AddNodeAdditionConstraint(GraphConstraint.NodeAddition c)
        {
            this.NodeAdditionConstraints.Add(c);
            return this;
        }

        public GraphBuilder AddEdgeAdditionConstraint(GraphConstraint.EdgeAddition c)
        {
            this.EdgeAdditionConstraints.Add(c);
            return this;
        }

        public GraphBuilder AddNodeRemovalConstraint(GraphConstraint.NodeRemoval c)
        {
            this.NodeRemovalConstraints.Add(c);
            return this;
        }

        public GraphBuilder AddEdgeRemovalConstraint(GraphConstraint.EdgeRemoval c)
        {
            this.EdgeRemovalConstraints.Add(c);
            return this;
        }

        public GraphBuilder DontAddDefaultConstraints()
        {
            this.ShouldAddDefaultConstraints = false;
            return this;
        }

        public Graph Build()
        {
            if (this.ShouldAddDefaultConstraints)
            {
                this.AddDefaultConstraints();
                this.ShouldAddDefaultConstraints = false;
            }
            return new Graph(this.NodeAdditionConstraints,
                             this.EdgeAdditionConstraints,
                             this.NodeRemovalConstraints,
                             this.EdgeRemovalConstraints);
        }

        private readonly List<GraphConstraint.NodeAddition> NodeAdditionConstraints = new List<GraphConstraint.NodeAddition>();
        private readonly List<GraphConstraint.EdgeAddition> EdgeAdditionConstraints = new List<GraphConstraint.EdgeAddition>();
        private readonly List<GraphConstraint.NodeRemoval> NodeRemovalConstraints = new List<GraphConstraint.NodeRemoval>();
        private readonly List<GraphConstraint.EdgeRemoval> EdgeRemovalConstraints = new List<GraphConstraint.EdgeRemoval>();
        private bool ShouldAddDefaultConstraints = true;
    }
}

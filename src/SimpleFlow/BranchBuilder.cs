using System.Xml.Linq;

namespace SimpleFlow
{
    /// <summary>
    /// A helper class to build branches in a flow.
    /// </summary>
    /// <typeparam name="TContext">Context Type.</typeparam>
    public class BranchBuilder<TContext>
        where TContext : IFlowContext
    {
        private readonly FlowOptions<TContext> _branchOptions;

        public BranchBuilder(FlowOptions<TContext> options)
        {
            _branchOptions = options;
        }

        /// <summary>
        /// Add a node in the end of the branch.
        /// </summary>
        /// <typeparam name="TProcess">Type of process.</typeparam>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="name">Node name.</param>
        /// <returns>Returns a helper class to build nodes in a flow.</returns>
        /// <exception cref="FlowException"></exception>
        public NodeBuilder<TContext> AddNext<TProcess>(string? identifier = null, string? name = null)
            where TProcess : IProcess<TContext>
        {
            // Generate a new identifier if not provided
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = Guid.NewGuid().ToString();
            }

            // Use the process name if the name is not provided
            if (string.IsNullOrWhiteSpace(name))
            {
                name = typeof(TProcess).Name;
            }

            var previousOptions = _branchOptions.GetPreviousBranchOptions();

            if (previousOptions == null)
            {
                throw new FlowException("Previous Flow Options not set.");
            }

            // Check if the next node already exists.
            // Nodes can be reprocessed in a return flow cenario, but for this case,
            // AddBranch should be used to prevent deadlocks.
            var nextNode = _branchOptions.GetNode(identifier);

            if (nextNode != null)
            {
                throw new FlowException($"Node with Identifier \"{identifier}\" already exists. " +
                    $"If your intention is to create a reprocess flow, use AddBranch instead.");
            }

            nextNode = new Node<TContext>
            {
                Branch = previousOptions.Branch,
                Identifier = identifier,
                Name = name,
                Type = NodeType.Simple,
                ProcessType = typeof(TProcess)
            };

            var lastBranchNodes = _branchOptions.GetLastNodes();

            foreach (var node in lastBranchNodes)
            {
                node.Next.Add(nextNode);
                nextNode.Previous.Add(node);
            }

            return previousOptions.AddNode(nextNode);
        }
    }
}

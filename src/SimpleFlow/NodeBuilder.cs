namespace SimpleFlow
{
    public class NodeBuilder<TContext>
        where TContext : IFlowContext
    {
        private readonly Node<TContext> _node;
        private readonly FlowOptions<TContext> _options;

        public NodeBuilder(Node<TContext> node, FlowOptions<TContext> options)
        {
            _node = node;
            _options = options;
        }

        /// <summary>
        /// Add a node that will be executed after the current node.
        /// </summary>
        /// <typeparam name="TProcess">Type of process that will be executed</typeparam>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="name">Node friendly name.</param>
        /// <returns>A <see cref="NodeBuilder{TContext}"/> instance.</returns>
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

            // Check if the next node already exists.
            // Nodes can be reprocessed in a return flow cenario, but for this case,
            // AddBranch should be used to prevent deadlocks.
            var nextNode = _options.GetNode(identifier);

            if (nextNode != null)
            {
                throw new FlowException($"Node with Identifier \"{identifier}\" already exists. " +
                    $"If your intention is to create a reprocess flow, use AddBranch instead.");
            }

            nextNode = new Node<TContext>
            {
                Branch = _options.Branch,
                Identifier = identifier,
                Name = name,
                Type = NodeType.Simple,
                ProcessType = typeof(TProcess)
            };

            _node.Next.Add(nextNode);
            nextNode.Previous.Add(_node);

            return _options.AddNode(nextNode);
        }

        /// <summary>
        /// Add a branch that will be executed after the current node
        /// </summary>
        /// <param name="name">Branch name.</param>
        /// <param name="configure">Branch configuration.</param>
        /// <returns>A <see cref="BranchBuilder{TContext}"/> instance.</returns>
        public BranchBuilder<TContext> AddBranch(string? name, Action<FlowOptions<TContext>> configure)
        {
            // Create a new FlowOptions instance for the branch.
            // This will allow the branch to have its own set of nodes.
            var branchOptions = new FlowOptions<TContext>();
            
            branchOptions.SetPreviousFlowOptions(_options);

            // Define de Branch Name. Branch names must be unique.
            if (string.IsNullOrEmpty(name))
                name = Guid.NewGuid().ToString();

            branchOptions.SetBranchName(name);

            // Apply branch options.
            configure(branchOptions);

            // Apply last configured node as previous node for the initial branch nodes.
            var initialNodes = branchOptions.GetBeginNodes();

            foreach(var node in initialNodes)
            {
                node.Previous.Add(_node);
                _node.Next.Add(node);
            }

            // Get all nodes and add them to the current options.
            var allNodes = branchOptions.GetNodes();

            _options.AddNodes(allNodes);

            return new BranchBuilder<TContext>(branchOptions);
        }

        public NodeBuilder<TContext> WithCondition(Func<TContext, bool> condition)
        {
            _node.Condition = condition;
            return this;
        }

        /// <summary>
        /// Define current node as the end of the flow.
        /// </summary>
        public void End()
        {
            _node.Type = NodeType.End;
        }
    }
}
namespace SimpleFlow
{
    /// <summary>
    /// Options to configure a flow.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class FlowOptions<TContext>
        where TContext : IFlowContext
    {
        private string _branch = "root";
        private FlowOptions<TContext>? _previousOptions;
        private readonly ICollection<Node<TContext>> _nodeContainer = [];

        public string Branch => _branch;

        /// <summary>
        /// Add a begin node to the flow in the current branch.
        /// </summary>
        /// <typeparam name="TProcess">Type of process that will be executed.</typeparam>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="name">A friendly node name.</param>
        /// <returns>A <see cref="NodeBuilder{TContext}"/> instance.</returns>
        public NodeBuilder<TContext> Begin<TProcess>(string? identifier = null, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = typeof(TProcess).Name;
            }

            // Create a begin node.
            // Begin nodes do not have previous nodes.
            var node = new Node<TContext>
            {
                Branch = _branch,
                Identifier = identifier,
                Name = name,
                Type = NodeType.Begin,
                ProcessType = typeof(TProcess)
            };

            return AddNode(node);
        }

        /// <summary>
        /// Insert a node in the node container of the current branch.
        /// </summary>
        /// <param name="node">Node to be inserted.</param>
        /// <returns>Returns a <see cref="NodeBuilder{TContext}"/> instance.</returns>
        public NodeBuilder<TContext> AddNode(Node<TContext> node)
        {
            _nodeContainer.Add(node);
            return new NodeBuilder<TContext>(node, this);
        }

        /// <summary>
        /// Insert a collection of nodes in the node container of the current branch.
        /// </summary>
        /// <param name="nodes">Nodes to be inserted.</param>
        public void AddNodes(ICollection<Node<TContext>> nodes)
        {
            foreach (var node in nodes)
            {
                _nodeContainer.Add(node);
            }
        }

        /// <summary>
        /// Get a node by it's identifier.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <returns>A <see cref="Node{TContext}"/>  that corresponds to passed identifier or <see cref="null"/> if node doesn't exists.</returns>
        public Node<TContext>? GetNode(string identifier)
        {
            return _nodeContainer.FirstOrDefault(n => n.Identifier == identifier);
        }

        /// <summary>
        /// Get all nodes in the current branch node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Node<TContext>> GetNodes()
        {
            return _nodeContainer;
        }

        /// <summary>
        /// Get all begin nodes in the current branch node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Node<TContext>> GetBeginNodes()
        {
            return _nodeContainer.Where(x => x.Type == NodeType.Begin && x.Previous.Count == 0).ToList();  
        }

        /// <summary>
        /// Get all begin nodes in the root node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Node<TContext>> GetRootBeginNodes()
        {
            return _nodeContainer.Where(x => x.Type == NodeType.Begin && x.Branch == "root").ToList();
        }

        /// <summary>
        /// Get all end nodes in the current branch node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Node<TContext>> GetEndNodes()
        {
            return _nodeContainer.Where(x => x.Type == NodeType.End).ToList();
        }

        /// <summary>
        /// Get all simple nodes that doesn't have a next node in the current branch node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Node<TContext>> GetLastNodes()
        {
            // Begin nodes with no next nodes are also considered last nodes.
            return _nodeContainer.Where(x => (x.Type == NodeType.Simple || x.Type == NodeType.Begin) && x.Next.Count == 0).ToList();
        }

        /// <summary>
        /// Get all process types in the current branch node container.
        /// </summary>
        /// <returns></returns>
        public ICollection<Type> GetProcessTypes()
        {
            return _nodeContainer.Select(n => n.ProcessType).ToList();
        }

        /// <summary>
        /// Get the root branch node options.
        /// </summary>
        /// <returns></returns>
        public FlowOptions<TContext> GetRootOptions()
        {
            if (_previousOptions == null)
            {
                return this;
            }

            return _previousOptions.GetRootOptions();
        }

        /// <summary>
        /// Change the current branch name.
        /// </summary>
        /// <param name="name"></param>
        public void SetBranchName(string name)
        {
            // Name validations.
            if (string.IsNullOrWhiteSpace(name))
                throw new FlowException("Branch name cannot be null or empty.");
            else if (name == "root")
                throw new FlowException("Branch name cannot be \"root\".");

            _branch = name;
        }

        /// <summary>
        /// Get options of the previous branch.
        /// </summary>
        /// <returns></returns>
        public FlowOptions<TContext>? GetPreviousBranchOptions()
        {
            return _previousOptions;
        }

        /// <summary>
        /// Set the previous branch options.
        /// </summary>
        /// <param name="options"></param>
        public void SetPreviousFlowOptions(FlowOptions<TContext> options)
        {
            _previousOptions = options;
        }
    }
}
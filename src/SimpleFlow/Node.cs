namespace SimpleFlow
{
    /// <summary>
    /// Represents a node in a flow.
    /// </summary>
    /// <typeparam name="TContext">Type of context that will be processed.</typeparam>
    public class Node<TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        /// Process instance that will be executed.
        /// </summary>
        private IProcess<TContext>? _process { get; set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        public required string Identifier { get; set; }

        /// <summary>
        /// Node branch.
        /// </summary>
        public required string Branch { get; set; }
        
        /// <summary>
        /// Node name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Node type.
        /// </summary>
        public required NodeType Type { get; set; }

        /// <summary>
        /// Type of process that will be executed.
        /// This is used to activate the process instance.
        /// </summary>
        public required Type ProcessType { get; set; }

        /// <summary>
        /// Node status.
        /// </summary>
        public NodeStatus Status { get; set; } = NodeStatus.NotStarted;

        public Func<TContext, bool>? Condition { get; set; }

        /// <summary>
        /// Previous nodes.
        /// </summary>
        public ICollection<Node<TContext>> Previous { get; } = [];

        /// <summary>
        /// Next nodes.
        /// </summary>
        public ICollection<Node<TContext>> Next { get; } = [];

        /// <summary>
        /// Sets the process instance that will be executed.
        /// </summary>
        /// <param name="process">Instance of the process.</param>
        public void SetProcessInstance(IProcess<TContext> process)
        {
            ArgumentNullException.ThrowIfNull(process, nameof(process));

            _process = process;
        }

        /// <summary>
        /// Runs the node with the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task RunAsync(TContext context)
        {
            // Run only all previous node status is completed
            if (Previous.Any(p => p.Status != NodeStatus.Completed && p.Type != NodeType.End))
            {
                return;
            }

            if (_process == null)
            {
                throw new InvalidOperationException("Process is not set.");
            }

            Status = NodeStatus.Running;

            // Test condition before execute the process
            if (Condition != null && !Condition(context))
            {
                Status = NodeStatus.Completed;
                context.AppendHistory(ProcessHistory.Success(Name, "Not executed. Did not pass the condition."));
                return;
            }

            // Execute the process
            await _process.ExecuteAsync(context);

            context.AppendHistory(ProcessHistory.Success(Name));

            Status = NodeStatus.Completed;

            // After running the process, run all next nodes.
            foreach (var node in Next)
            {
                await node.RunAsync(context);
            }
        }
    }
}
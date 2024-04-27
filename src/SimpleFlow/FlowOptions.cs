namespace SimpleFlow
{
    public class FlowOptions<TContext>
        where TContext : IFlowContext
    {
        private readonly ICollection<FlowNode<TContext>> _nodes = [];

        public FlowNodeBuilder<TContext> AddNode<TProcess>(string? identifier = null, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = typeof(TProcess).Name;
            }

            var node = new FlowNode<TContext>
            {
                Identifier = identifier,
                Name = name,
                ProcessType = typeof(TProcess)
            };

            _nodes.Add(node);

            return new FlowNodeBuilder<TContext>(node);
        }

        public ICollection<Type> GetProcessTypes()
        {
            return _nodes.Select(n => n.ProcessType).ToList();
        }
    }
}

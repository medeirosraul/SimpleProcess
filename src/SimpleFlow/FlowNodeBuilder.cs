namespace SimpleFlow
{
    public class FlowNodeBuilder<TContext>
        where TContext : IFlowContext
    {
        private readonly FlowNode<TContext> _node;

        public FlowNodeBuilder(FlowNode<TContext> node)
        {
            _node = node;
        }

        public FlowNodeBuilder<TContext> AddNext<TProcess>(string? identifier = null, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = typeof(TProcess).Name;
            }

            var nextNode = new FlowNode<TContext>
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = name,
                ProcessType = typeof(TProcess)
            };

            _node.Previous.Add(_node);
            _node.Next.Add(nextNode);

            return new FlowNodeBuilder<TContext>(nextNode);
        }
    }
}

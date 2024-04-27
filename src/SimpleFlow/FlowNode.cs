namespace SimpleFlow
{
    public class FlowNode<TContext>
        where TContext : IFlowContext
    {
        private IProcess<TContext>? _process { get; set; }

        public required string Identifier { get; set; }
        public required string Name { get; set; }
        public required Type ProcessType { get; set; }
        public FlowNodeStatus Status { get; set; } = FlowNodeStatus.NotStarted;
        public ICollection<FlowNode<TContext>> Previous { get; } = [];
        public ICollection<FlowNode<TContext>> Next { get; } = [];

        public async Task RunAsync(TContext context)
        {
            // Run only all previous node status is completed
            if (Previous.Any(p => p.Status != FlowNodeStatus.Completed))
            {
                return;
            }

            if (_process == null)
            {
                throw new InvalidOperationException("Process is not set.");
            }

            Status = FlowNodeStatus.Running;

            await _process.ExecuteAsync(context);

            Status = FlowNodeStatus.Completed;

            foreach (var node in Next)
            {
                await node.RunAsync(context);
            }
        }
    }
}

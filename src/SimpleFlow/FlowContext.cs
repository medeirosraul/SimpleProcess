
namespace SimpleFlow
{
    public abstract class FlowContext : IFlowContext
    {
        public ICollection<ProcessHistory> History { get; } = [];

        public void AppendHistory(ProcessHistory history)
        {
            History.Add(history);
        }
    }
}

namespace SimpleFlow
{
    public interface IFlowContext
    {
        ICollection<ProcessHistory> History { get; }
        void AppendHistory(ProcessHistory history);
    }
}

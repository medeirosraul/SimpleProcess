namespace SimpleFlow
{
    public interface IProcess<TContext>
        where TContext : IFlowContext
    {
        Task ExecuteAsync(TContext context);
    }
}

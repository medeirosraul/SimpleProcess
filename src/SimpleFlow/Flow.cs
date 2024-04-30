using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimpleFlow
{
    /// <summary>
    /// A flow is a collection of processes that are executed in a specific order.
    /// </summary>
    /// <typeparam name="TContext">Type of context that will be processed.</typeparam>
    public class Flow<TContext>
        where TContext : IFlowContext
    {
        private readonly string _name;
        private readonly FlowOptions<TContext> _options;
        private readonly IServiceProvider _serviceProvider;
        
        public Flow(IOptionsSnapshot<FlowOptions<TContext>> options, IServiceProvider serviceProvider, string name)
        {
            _name = name;
            _options = options.Get(name);
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Initializes the flow by activating all process types.
        /// </summary>
        private void Configure()
        {
            // Activate all process types.
            _options.GetNodes().ToList().ForEach(node =>
            {
                var process = (IProcess<TContext>)_serviceProvider.GetRequiredService(node.ProcessType);
                node.SetProcessInstance(process);
            });
        }

        /// <summary>
        /// Runs the flow with the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task RunAsync(TContext context)
        {
            Configure();

            // Get initial nodes and run them.
            var initialNodes = _options.GetRootBeginNodes();

            foreach(var node in initialNodes)
            {
                await node.RunAsync(context);
            }
        }
    }
}
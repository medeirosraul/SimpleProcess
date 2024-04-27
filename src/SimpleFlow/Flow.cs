using Microsoft.Extensions.Options;

namespace SimpleFlow
{
    public class Flow<TContext>
        where TContext : IFlowContext
    {
        public string Name => _options.Name;

        private readonly FlowOptions<TContext> _options;
        private readonly IServiceProvider _serviceProvider;
        
        public Flow(IOptions<FlowOptions<TContext>> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }
    }
}
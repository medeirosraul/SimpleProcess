﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleFlow
{
    public static class FlowExtensions
    {
        public static IServiceCollection AddFlow<TContext>(this IServiceCollection services, string name, Action<FlowOptions<TContext>> configure)
            where TContext : IFlowContext
        {
            // Add Flow as a keyed service into the DI container
            services.AddKeyedScoped<Flow<TContext>>(name);

            // Configure the FlowOptions<TContext> with the provided configuration
            services.Configure(name, configure);

            var options = new FlowOptions<TContext>();

            configure(options);

            // Register all node processes in the DI container
            foreach (var type in options.GetProcessTypes())
            {
                services.TryAddTransient(type);
            }

            return services;
        }
    }
}
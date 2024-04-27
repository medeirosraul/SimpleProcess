using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class InitProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            // Simulate getting product from cart
            context.Products.Add(new SaleProduct { Name = "Product 1", Value = 100 });
            context.Products.Add(new SaleProduct { Name = "Product 2", Value = 200 });
            context.Products.Add(new SaleProduct { Name = "Product 3", Value = 300 });

            return Task.CompletedTask;
        }
    }
}
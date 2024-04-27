using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class DiscountProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            var taxPercentage = 0.25m;

            var tax = (context.Value - context.Discount) * taxPercentage;

            context.Discounts.Add(new SaleDiscount
            {
                Name = "Coupom discount",
                Value = tax
            });

            return Task.CompletedTask;
        }
    }
}

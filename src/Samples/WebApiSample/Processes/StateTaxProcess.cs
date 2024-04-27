using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class StateTaxProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            var taxPercentage = 0.05m;

            var tax = (context.Value - context.Discount) * taxPercentage;

            context.Taxes.Add(new SaleTax
            {
                Name = "State Tax",
                Value = tax
            });

            return Task.CompletedTask;
        }
    }
}

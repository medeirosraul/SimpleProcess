using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class MunicipalTaxProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            var taxPercentage = 0.1m;

            var tax = (context.Value - context.Discount) * taxPercentage;

            context.Taxes.Add(new SaleTax
            {
                Name = "Municipal Tax",
                Value = tax
            });

            return Task.CompletedTask;
        }
    }
}

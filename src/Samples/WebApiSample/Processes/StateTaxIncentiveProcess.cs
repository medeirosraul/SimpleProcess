using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class StateTaxIncentiveProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            // Apply state tax isention
            var tax = context.Taxes.Where(t => t.Name == "State Tax").FirstOrDefault();
            
            if (tax != null)
            {
                tax.Value = 0;
            }

            return Task.CompletedTask;
        }
    }
}

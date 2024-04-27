using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class CheckoutProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            throw new NotImplementedException();
        }
    }
}

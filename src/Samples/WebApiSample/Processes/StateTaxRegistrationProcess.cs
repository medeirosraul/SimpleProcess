using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class StateTaxRegistrationProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            // Simulate state tax registration.
            Console.WriteLine("State tax registration process executed.");

            return Task.CompletedTask;
        }
    }
}

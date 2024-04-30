using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Processes
{
    public class UpdateDatabaseProcess : IProcess<SaleContext>
    {
        public Task ExecuteAsync(SaleContext context)
        {
            // Simulates a database update.
            Console.WriteLine("Database updated.");

            return Task.CompletedTask;
        }
    }
}

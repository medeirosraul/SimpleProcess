using Microsoft.AspNetCore.Mvc;
using SimpleFlow;
using WebApiSample.Processes.Contexts;

namespace WebApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly Flow<SaleContext> flow;

        public CheckoutController(ILogger<CheckoutController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            flow = serviceProvider.GetRequiredKeyedService<Flow<SaleContext>>("SaleProcessing");
        }

        [HttpGet(Name = "Get")]
        public async Task<SaleContext> Get()
        {
            var sale = new SaleContext();

            await flow.RunAsync(sale);

            _logger.LogInformation("Checkout completed.");

            return sale;
        }
    }
}

using SimpleFlow;
using WebApiSample.Processes;
using WebApiSample.Processes.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Process
builder.Services.AddFlow<SaleContext>("SaleProcessing", options =>
{
    options
        .Begin<InitProcess>("Init")
        .AddNext<DiscountProcess>("Discount")
        .AddBranch("Taxes", taxesBranch =>
        {
            taxesBranch.Begin<MunicipalTaxProcess>("MunicipalTax").WithCondition(ctx => !ctx.MunicipalTaxIsent)
                .AddNext<UpdateDatabaseProcess>("UpdateMunicipalTax");
            taxesBranch.Begin<StateTaxProcess>("StateTax")
                .AddBranch("StateTaxBranch", stateTaxBranch =>
                {
                    stateTaxBranch.Begin<StateTaxIncentiveProcess>("StateTaxIncentive");
                    stateTaxBranch.Begin<StateTaxRegistrationProcess>();
                })
                .AddNext<UpdateDatabaseProcess>("UpdateStateTax");
        })
        .AddNext<CheckoutProcess>("Checkout");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

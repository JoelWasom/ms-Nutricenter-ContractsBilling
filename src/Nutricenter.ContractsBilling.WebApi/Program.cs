using Nutricenter.ContractsBilling.Application.Service;
using Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices;
using Nutricenter.ContractsBilling.Infrastructure;
using Nutricenter.ContractsBilling.Infrastructure.ExternalServices.Catering;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CateringOptions>(
    builder.Configuration.GetSection("JsonCateringDataPath"));
builder.Services.AddScoped<IServiceCatalog, JsonCateringProvider>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

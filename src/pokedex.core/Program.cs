using FastEndpoints;
using Pokedex.Core.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddJsonServices();
builder.Services.AddFastEndpoints();

// Dependency Injection
builder.AddApplicationServices();
builder.AddInfrastructureServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseFastEndpoints();

await app.RunAsync();

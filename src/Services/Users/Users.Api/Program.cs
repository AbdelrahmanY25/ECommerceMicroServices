var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddApiServices(builder.Configuration)
	.AddApplicationServices()
	.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
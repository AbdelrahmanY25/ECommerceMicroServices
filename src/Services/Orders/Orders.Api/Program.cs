var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddBusinessLogicLayer()
	.AddDataAccessLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{
	string host = builder.Configuration["USERS_MICROSERVICE_HOST"]!;
	string port = builder.Configuration["USERS_MICROSERVICE_PORT"]!;

	client.BaseAddress = new Uri($"http://{host}:{port}");
})
.AddPolicyHandler(
	builder.Services.BuildServiceProvider()
		.GetRequiredService<IUserMicroservicePolicies>()
		.GetRetryPolicy()
)
.AddPolicyHandler(
	builder.Services.BuildServiceProvider()
		.GetRequiredService<IUserMicroservicePolicies>()
		.GetCircuitBreakerPolicy()
);

builder.Services.AddHttpClient<ProductsMicroserviceClient>(client =>
{
	string host = builder.Configuration["PRODUCTS_MICROSERVICE_HOST"]!;
	string port = builder.Configuration["PRODUCTS_MICROSERVICE_PORT"]!;

	client.BaseAddress = new Uri($"http://{host}:{port}");
})
.AddPolicyHandler(
	builder.Services.BuildServiceProvider()
		.GetRequiredService<IProductMicroservicePolicies>()
		.GetFallbackPolicy()
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
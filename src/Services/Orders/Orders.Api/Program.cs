var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddBusinessLogicLayer()
	.AddDataAccessLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddHttpClient("UsersMicroservice", client =>
{	
	client.BaseAddress = new Uri(builder.Configuration
		.GetValue<string>($"https://{builder.Configuration["USERS_MICROSERVICE_HOST"]}:{builder.Configuration["USERS_MICROSERVICE_PORT"]}")!);
});

builder.Services.AddScoped<IUsersMicroserviceClient, UsersMicroserviceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
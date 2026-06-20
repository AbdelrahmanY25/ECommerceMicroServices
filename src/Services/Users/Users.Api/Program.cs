var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependancies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

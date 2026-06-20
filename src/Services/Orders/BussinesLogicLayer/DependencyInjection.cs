namespace BussinesLogicLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddBusinessLogicLayer(IConfiguration configuration)
		{
			services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

			services.AddScoped<IOrdersService, OrdersService>();

			return services;
		}
	}
}
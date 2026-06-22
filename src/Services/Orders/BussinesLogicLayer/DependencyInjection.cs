namespace BussinesLogicLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddBusinessLogicLayer()
		{
			services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

			services.AddScoped<IOrdersService, OrdersService>();

			return services;
		}
	}
}
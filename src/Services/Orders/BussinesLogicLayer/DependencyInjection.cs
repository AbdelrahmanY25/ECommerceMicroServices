namespace BussinesLogicLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddBusinessLogicLayer()
		{
			services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

			services.AddScoped<IOrdersService, OrdersService>();

			services.AddTransient<IUserMicroservicePolicies, UserMicroservicePolicies>();

			services.AddTransient<IProductMicroservicePolicies, ProductMicroservicePolicies>();

			return services;
		}
	}
}
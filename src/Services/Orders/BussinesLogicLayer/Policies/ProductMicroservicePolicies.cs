namespace BussinesLogicLayer.Policies;

public class ProductMicroservicePolicies(ILogger<ProductMicroservicePolicies> logger) : IProductMicroservicePolicies
{
	private readonly ILogger<ProductMicroservicePolicies> _logger = logger;

	public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
	{
		 AsyncFallbackPolicy<HttpResponseMessage> fallbackPolicy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.FallbackAsync(async (context) =>
			{
				_logger.LogWarning("Fallback triggered: The request failed, returning dummy data");

				var product = new ProductResponse(ProductID: Guid.Empty,
				  ProductName: "Temporarily Unavailable (fallback)",
				  Category: "Temporarily Unavailable (fallback)",
				  UnitPrice: 0,
				  QuantityInStock: 0
				  );

				var response = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json")
				};

				return response;
			});

		return fallbackPolicy;
	}
}
namespace BussinesLogicLayer.HttpClients.Products;

public class ProductsMicroserviceClient(HttpClient httpClient, IDistributedCache distributedCache)
{
	private readonly HttpClient _httpClient = httpClient;
	private readonly IDistributedCache _distributedCache = distributedCache;

	public async Task<ProductResponse?> GetProductById(Guid productID)
	{
		string? cachedProduct = await _distributedCache.GetStringAsync($"product-{productID}");
		
		if (string.IsNullOrEmpty(cachedProduct))
		{
			ProductResponse? productResponse = await _httpClient
				.GetFromJsonAsync<ProductResponse>($"/api/products/search/product-id/{productID}");
			
			if (productResponse is not null)
				await _distributedCache.SetStringAsync($"product-{productID}", JsonSerializer.Serialize(productResponse));

			return productResponse is null ? throw new ArgumentException("Invalid Product ID") : productResponse;
		}
		
		return JsonSerializer.Deserialize<ProductResponse>(cachedProduct);
	}
}
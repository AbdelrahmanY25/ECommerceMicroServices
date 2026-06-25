namespace BussinesLogicLayer.HttpClients.Products;

public class ProductsMicroserviceClient(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<ProductResponse?> GetProductById(Guid productID)
	{
		ProductResponse? productResponse = await _httpClient.GetFromJsonAsync<ProductResponse>($"/api/products/search/product-id/{productID}");

		return productResponse is null ? throw new ArgumentException("Invalid Product ID") : productResponse;
	}
}
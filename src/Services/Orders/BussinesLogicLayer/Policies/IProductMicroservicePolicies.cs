namespace BussinesLogicLayer.Policies;

public interface IProductMicroservicePolicies
{
	IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
}
namespace BussinesLogicLayer.Policies;

public interface IUserMicroservicePolicies
{
	IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
}
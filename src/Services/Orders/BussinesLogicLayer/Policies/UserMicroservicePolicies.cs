namespace BussinesLogicLayer.Policies;

public class UserMicroservicePolicies(IPollyPolicies pollyPolicies) : IUserMicroservicePolicies
{
	private readonly IPollyPolicies _pollyPolicies = pollyPolicies;

	public IAsyncPolicy<HttpResponseMessage> GetCompinePolicy()
	{
		var retryPolicy = _pollyPolicies.GetRetryPolicy(3);
		var circuitBreakerPolicy = _pollyPolicies.GetCircuitBreakerPolicy(3, TimeSpan.FromMinutes(2));
		var timeoutPolicy = _pollyPolicies.GetTimeoutPolicy(TimeSpan.FromMilliseconds(1500));

		AsyncPolicyWrap<HttpResponseMessage> wrappedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
		return wrappedPolicy;
	}
}
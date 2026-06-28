namespace BussinesLogicLayer.Policies;

public class UserMicroservicePolicies(ILogger<UserMicroservicePolicies> logger) : IUserMicroservicePolicies
{
	public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
		.WaitAndRetryAsync(
			retryCount: 3,
			sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2),
			onRetry: (outcome, timespan, retryAttempt, context) =>
			{
				logger.LogWarning("Retry {RetryAttempt} for {OperationKey} due to {StatusCode}. Waiting {Delay} seconds before next retry.",
					retryAttempt,
					context.OperationKey,
					outcome.Result?.StatusCode,
					timespan.TotalSeconds);
			}
		);

		return policy;
	}
}

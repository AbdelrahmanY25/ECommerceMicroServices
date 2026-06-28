using Polly.CircuitBreaker;

namespace BussinesLogicLayer.Policies;

public class UserMicroservicePolicies(ILogger<UserMicroservicePolicies> logger) : IUserMicroservicePolicies
{
	public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		AsyncRetryPolicy<HttpResponseMessage> policy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.WaitAndRetryAsync(
				retryCount: 3, // Number of retries
				sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Wait time between retries Exponential backoff
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

	public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
	{
		AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.CircuitBreakerAsync(
				handledEventsAllowedBeforeBreaking: 3, // Number of failed requests before the circuit breaker trips
				durationOfBreak: TimeSpan.FromMinutes(2), // Duration to half-open the circuit breaker before allowing requests again
				onBreak: (outcome, breakDelay) =>
				{
					logger.LogWarning("Circuit breaker opened due to {StatusCode}. Break duration: {BreakDuration} seconds.",
						outcome.Result?.StatusCode,
						breakDelay.TotalSeconds);
				},
				onReset: () =>
				{
					logger.LogInformation("Circuit breaker reset. Requests will be allowed again.");
				}
			);

			return policy;
	}
}

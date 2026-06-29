namespace BussinesLogicLayer.Policies;

public class PollyPolicies(ILogger<UserMicroservicePolicies> logger) : IPollyPolicies
{
	private readonly ILogger<UserMicroservicePolicies> _logger = logger;

	public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
	{
		AsyncRetryPolicy<HttpResponseMessage> policy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.WaitAndRetryAsync(
				retryCount: retryCount, // Number of retries
				sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Wait time between retries Exponential backoff
				onRetry: (outcome, timespan, retryAttempt, context) =>
				{
					_logger.LogWarning("Retry {RetryAttempt} for {OperationKey} due to {StatusCode}. Waiting {Delay} seconds before next retry.",
						retryAttempt,
						context.OperationKey,
						outcome.Result?.StatusCode,
						timespan.TotalSeconds);
				}
			);

		return policy;
	}

	public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
	{
		AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.CircuitBreakerAsync(
				handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking, // Number of failed requests before the circuit breaker trips
				durationOfBreak: durationOfBreak, // Duration to half-open the circuit breaker before allowing requests again
				onBreak: (outcome, breakDelay) =>
				{
					_logger.LogWarning("Circuit breaker opened due to {StatusCode}. Break duration: {BreakDuration} seconds.",
						outcome.Result?.StatusCode,
						breakDelay.TotalSeconds);
				},
				onReset: () =>
				{
					_logger.LogInformation("Circuit breaker reset. Requests will be allowed again.");
				}
			);

		return policy;
	}

	public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout)
	{
		AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(timeout);

		return policy;
	}
}
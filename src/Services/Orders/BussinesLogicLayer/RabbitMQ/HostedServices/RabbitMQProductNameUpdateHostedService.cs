namespace BussinesLogicLayer.RabbitMQ.HostedServices;

internal class RabbitMQProductNameUpdateHostedService(IRabbitMQProductNameUpdateConsumer productNameUpdateConsumer) : IHostedService
{
	private readonly IRabbitMQProductNameUpdateConsumer _productNameUpdateConsumer = productNameUpdateConsumer;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await _productNameUpdateConsumer.ConsumeAsync();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_productNameUpdateConsumer.Dispose();

		return Task.CompletedTask;
	}
}
namespace BussinesLogicLayer.RabbitMQ;

public class RabbitMQProductNameUpdateConsumer : IRabbitMQProductNameUpdateConsumer, IDisposable
{
	private readonly ILogger _logger;
	private readonly IChannel _channel;
	private readonly IConnection _connection;
	private readonly IConfiguration _configuration;

	public RabbitMQProductNameUpdateConsumer(ILogger<RabbitMQProductNameUpdateConsumer> logger, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;

		string HostName = _configuration.GetSection("RabbitMQ:HostName").Value!;
		string UserName = _configuration.GetSection("RabbitMQ:UserName").Value!;
		string Password = _configuration.GetSection("RabbitMQ:Password").Value!;
		string Port = _configuration.GetSection("RabbitMQ:Port").Value!;

		ConnectionFactory connectionFactory = new()
		{
			HostName = HostName,
			UserName = UserName,
			Password = Password,
			Port = int.Parse(Port)
		};

		_connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();

		_channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
	}

	public async Task Consume()
	{
		string routingKey = "product.update.name";

		string queueName = "orders.product.update.name.queue";

		string exchangeName = _configuration.GetSection("RabbitMQ:Products:Exchange").Value!;

		await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

		await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

		await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

		AsyncEventingBasicConsumer consumer = new(_channel);

		consumer.ReceivedAsync += async (sender, eventArgs) =>
		{
			string messageJson = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

			var productNameUpdateMessage = JsonSerializer.Deserialize<ProductNameUpdateMessage>(messageJson);
			
			if (productNameUpdateMessage is not null)
				_logger.LogInformation("Received Product Name Update Message: ProductID={0}, NewProductName={1}", productNameUpdateMessage.ProductID, productNameUpdateMessage.NewProductName);
			
		};
		
		await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
	}

	public void Dispose()
	{
		_channel?.Dispose();
		_connection?.Dispose();
	}
}
namespace BussinesLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
	private readonly IChannel _channel;
	private readonly IConnection _connection;
	private readonly IConfiguration _configuration;

	public RabbitMQPublisher(IConfiguration configuration)
	{
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

	public async Task Publish<T>(string routingKey, T message)
	{
		string messageJson = JsonSerializer.Serialize(message);
		byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

		string exchangeName = _configuration.GetSection("RabbitMQ:Products:Exchange").Value!;

		await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

		await _channel.BasicPublishAsync(exchangeName, routingKey, messageBodyInBytes);
	}

	public void Dispose()
	{
		_channel.Dispose();
		_connection.Dispose();
	}
}
namespace BussinesLogicLayer.RabbitMQ;

public interface IRabbitMQPublisher
{
	Task Publisher<T>(string routingKey, T message);
}
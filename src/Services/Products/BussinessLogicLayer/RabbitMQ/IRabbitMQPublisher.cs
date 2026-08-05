namespace BussinesLogicLayer.RabbitMQ;

public interface IRabbitMQPublisher
{
	Task Publish<T>(string routingKey, T message);
}
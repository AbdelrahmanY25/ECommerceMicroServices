namespace BussinesLogicLayer.RabbitMQ;

public interface IRabbitMQProductNameUpdateConsumer
{
	Task ConsumeAsync();
	void Dispose();
}
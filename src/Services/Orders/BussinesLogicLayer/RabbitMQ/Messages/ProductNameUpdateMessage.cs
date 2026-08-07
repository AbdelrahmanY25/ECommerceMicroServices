namespace BussinesLogicLayer.RabbitMQ.Messages;

public record ProductNameUpdateMessage(Guid ProductID, string NewProductName);
namespace BussinesLogicLayer.Contracts;

public record OrderItemAddRequest
(
	Guid ProductID,
	decimal UnitPrice,
	int Quantity
);
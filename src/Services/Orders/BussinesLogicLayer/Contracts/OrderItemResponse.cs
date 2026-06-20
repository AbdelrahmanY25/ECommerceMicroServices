namespace BussinesLogicLayer.Contracts;

public record OrderItemResponse
(
	Guid ProductID,
	decimal UnitPrice,
	int Quantity,
	decimal TotalPrice
);
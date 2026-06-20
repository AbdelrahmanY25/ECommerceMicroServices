namespace BussinesLogicLayer.Contracts;

public record OrderItemUpdateRequest
(
	Guid ProductID,
	decimal UnitPrice,
	int Quantity
);
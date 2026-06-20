namespace BussinesLogicLayer.Contracts;

public record OrderUpdateRequest
(
	Guid OrderID,
	Guid UserID,
	DateTime OrderDate,
	List<OrderItemUpdateRequest> OrderItems
);
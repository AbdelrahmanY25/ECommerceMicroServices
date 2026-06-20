namespace BussinesLogicLayer.Contracts;

public record OrderAddRequest
(
	Guid UserID,
	DateTime OrderDate,
	List<OrderItemAddRequest> OrderItems
);
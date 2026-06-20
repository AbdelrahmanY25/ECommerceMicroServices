namespace BussinesLogicLayer.Contracts;

public record OrderResponse
(
	Guid OrderID,
	Guid UserID,
	decimal TotalBill,
	DateTime OrderDate,
	List<OrderItemResponse> OrderItems
);
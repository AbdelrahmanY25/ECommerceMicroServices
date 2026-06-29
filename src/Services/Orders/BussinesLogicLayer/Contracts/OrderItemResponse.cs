namespace BussinesLogicLayer.Contracts;

public record OrderItemResponse(Guid ProductID, decimal UnitPrice, int Quantity, decimal TotalPrice)
{
	public string? ProductName { get; set; }
	public string? Category { get; set; }
}
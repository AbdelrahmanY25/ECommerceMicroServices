namespace BussinessLogicLayer.Contracts;

public record ProductResponse
(
	Guid ProductID,
	string ProductName,
	CategoryOptions Category,
	double? UnitPrice,
	int? QuantityInStock
);
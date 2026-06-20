namespace BussinessLogicLayer.Contracts;

public record ProductAddRequest
(
	string ProductName,
	CategoryOptions Category,
	double? UnitPrice,
	int? QuantityInStock
);
namespace BussinessLogicLayer.Contracts;

public record ProductUpdateRequest
(
	Guid ProductID,
	string ProductName,
	CategoryOptions Category,
	double? UnitPrice,
	int? QuantityInStock
);
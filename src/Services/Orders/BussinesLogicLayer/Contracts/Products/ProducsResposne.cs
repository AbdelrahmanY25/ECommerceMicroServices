namespace BussinesLogicLayer.Contracts.Products;

public record ProductResponse(Guid ProductID, string? ProductName, string? Category, double UnitPrice, int QuantityInStock);
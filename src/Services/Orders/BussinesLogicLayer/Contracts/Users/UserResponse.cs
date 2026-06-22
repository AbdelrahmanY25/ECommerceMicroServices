namespace BussinesLogicLayer.Contracts.Users;

public record UserResponse(Guid UserID, string? Email, string? PersonName, string Gender);
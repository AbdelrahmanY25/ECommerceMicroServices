namespace Users.Application.Contracts;

public record UserResponse(Guid UserID, string? Email, string? PersonName, string Gender);
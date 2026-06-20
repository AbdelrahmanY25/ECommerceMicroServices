namespace Users.Application.Contracts;

public record RegisterRequest(
  string? Email,
  string? Password,
  string? PersonName,
  GenderOptions Gender
);
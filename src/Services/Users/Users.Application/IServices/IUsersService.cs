namespace Users.Application.IServices;

public interface IUsersService
{
	Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
	Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
	Task<UserResponse?> GetUserById(Guid? userID);
}
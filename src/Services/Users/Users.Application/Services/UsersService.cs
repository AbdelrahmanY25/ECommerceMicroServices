namespace Users.Application.Services;

internal class UsersService(IUsersRepository usersRepository) : IUsersService
{
	private readonly IUsersRepository _usersRepository = usersRepository;

	public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
	{
		ApplicationUser? user = await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

		if (user is null)
			return null;

		return user.Adapt<AuthenticationResponse>() with { Token = "token", Success = true };
	}

	public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
	{
		ApplicationUser user = new()
		{
			PersonName = registerRequest.PersonName,
			Email = registerRequest.Email,
			Password = registerRequest.Password,
			Gender = registerRequest.Gender.ToString()
		};

		ApplicationUser? registeredUser = await _usersRepository.AddUser(user);
		
		if (registeredUser is null)
			return null;

		return registeredUser.Adapt<AuthenticationResponse>() with { Token = "token", Success = true};
	}
}
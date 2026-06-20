namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUsersService usersService) : ControllerBase
{
	private readonly IUsersService _usersService = usersService;

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterRequest registerRequest)
	{
		if (registerRequest is null)
			return BadRequest("Invalid registration data");

		AuthenticationResponse? authenticationResponse = await _usersService.Register(registerRequest);

		if (authenticationResponse is null || authenticationResponse.Success == false)
			return BadRequest(authenticationResponse);

		return Ok(authenticationResponse);
	}


	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginRequest loginRequest)
	{
		if (loginRequest is null)
			return BadRequest("Invalid login data");

		AuthenticationResponse? authenticationResponse = await _usersService.Login(loginRequest);

		if (authenticationResponse is null || authenticationResponse.Success == false)
			return Unauthorized(authenticationResponse);

		return Ok(authenticationResponse);
	}
}
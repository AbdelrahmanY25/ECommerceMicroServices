namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService usersService) : ControllerBase
{
	private readonly IUsersService _usersService = usersService;

	[HttpGet("{userID}")]
	public async Task<IActionResult> GetUserByUserID(Guid userID)
	{
		await Task.Delay(1000);

		throw new Exception("This is a test exception for demonstration purposes.");

		if (userID == Guid.Empty)
			return BadRequest("Invalid User ID");

		UserResponse? response = await _usersService.GetUserById(userID);

		if (response is null)
			return NotFound(response);

		return Ok(response);
	}
}
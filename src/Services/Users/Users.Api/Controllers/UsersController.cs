namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService usersService) : ControllerBase
{
	private readonly IUsersService _usersService = usersService;

	[HttpGet("{userID}")]
	public async Task<IActionResult> GetUserByUserID(Guid userID)
	{
		if (userID == Guid.Empty)
			return BadRequest("Invalid User ID");

		UserResponse? response = await _usersService.GetUserById(userID);

		if (response is null)
			return NotFound(response);

		return Ok(response);
	}
}
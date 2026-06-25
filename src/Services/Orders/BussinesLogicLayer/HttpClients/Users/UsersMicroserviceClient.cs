namespace BussinesLogicLayer.HttpClients.Users;

public class UsersMicroserviceClient(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<UserResponse?> GetUserById(Guid userID)
	{		
		UserResponse? userResponse = await _httpClient.GetFromJsonAsync<UserResponse>($"/api/users/{userID}");

		return userResponse is null ? throw new ArgumentException("Invalid User ID") : userResponse;
	}
}
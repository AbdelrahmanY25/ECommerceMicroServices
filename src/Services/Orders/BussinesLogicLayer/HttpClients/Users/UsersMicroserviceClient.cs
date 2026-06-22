namespace BussinesLogicLayer.HttpClients.Users;

public class UsersMicroserviceClient(IHttpClientFactory httpClientFactory) : IUsersMicroserviceClient
{
	public async Task<UserResponse?> GetUserById(Guid userID)
	{
		var httpClient = httpClientFactory.CreateClient("UsersMicroservice");
		
		HttpResponseMessage response = await httpClient.GetAsync($"/api/users/{userID}");

		if (!response.IsSuccessStatusCode)
		{
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}
			else if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
			}
			else
			{
				throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");
			}
		}

		UserResponse? userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

		return userResponse is null ? throw new ArgumentException("Invalid User ID") : userResponse;
	}
}
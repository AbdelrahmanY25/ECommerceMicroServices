namespace BussinesLogicLayer.HttpClients.Users;

public interface IUsersMicroserviceClient
{
	Task<UserResponse?> GetUserById(Guid userID);
}
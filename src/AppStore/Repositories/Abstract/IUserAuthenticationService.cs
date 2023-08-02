namespace AppStore.Repositories.Abstract;

using AppStore.Models.DTO;
public interface IUserAuthenticationService
{
    Task<Status> LoginAsync(LoginModel login);

    Task LogoutAsync();
}
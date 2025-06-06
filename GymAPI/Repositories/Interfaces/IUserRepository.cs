using Core;

namespace GymAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> TryLoginAsync(string email, string password);
    Task<int> RegisterAsync(User user);
    Task<int> GetMaxId();
}
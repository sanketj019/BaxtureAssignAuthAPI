using BaxtureAssignAuthAPI.Models;

namespace BaxtureAssignAuthAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string userId);
        Task CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(string UserId);
        bool UserExists(string id);
        Task<List<User>> SearchUsersAsync(UserSearchRequest searchRequest);
    }
}

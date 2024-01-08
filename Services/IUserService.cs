using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserTasksBackend.Models;

namespace UserTasksBackend.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<UserWithTaskCount>> GetUsersWithTaskCountAsync();
        Task<User> AddUserAsync(User user);
    }
}

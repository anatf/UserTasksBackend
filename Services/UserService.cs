using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserTasksBackend.Models;

using ThreadingTask = System.Threading.Tasks.Task;

namespace UserTasksBackend.Services
{
    public class UserService : IUserService
    {
        private readonly UserTasksDbContext _dbContext;

        public UserService(UserTasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private int nextUserId = 1;

        public async Task<User> GetUserAsync(int userId)
        {
            return await ThreadingTask.Run(() => _dbContext.Users.FirstOrDefault(u => u.Id == userId));
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await ThreadingTask.Run(() => _dbContext.Users.AsEnumerable());
        }

        public async Task<User> AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return await ThreadingTask.Run(() => user);
        }

        public async Task<IEnumerable<UserWithTaskCount>> GetUsersWithTaskCountAsync()
        {
            return await ThreadingTask.Run(() =>
            {
                var result = from user in _dbContext.Users
                             join task in _dbContext.Tasks on user.Id equals task.UserId into userTasks
                             select new UserWithTaskCount
                             {
                                 Id = user.Id,
                                 Name = user.Name,
                                 TaskCount = userTasks.Count()
                             };

                return result.ToList<UserWithTaskCount>();
            });
        }
    }
}

//using UserTasksBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserTasksBackend.Models;
using ModelTask = UserTasksBackend.Models.Task;
using ThreadingTask = System.Threading.Tasks.Task;
namespace UserTasksBackend.Services
{
    public class TaskService : ITaskService
    {
        private readonly UserTasksBackend.Models.UserTasksDbContext _dbContext;

        public TaskService(UserTasksBackend.Models.UserTasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ModelTask> GetTaskAsync(int taskId)
        {
            return await ThreadingTask.Run(() => _dbContext.Tasks.FirstOrDefault(t => t.Id == taskId));
        }

        public async Task<IEnumerable<ModelTask>> GetTasksAsync()
        {
            return await ThreadingTask.Run(() => _dbContext.Tasks.AsEnumerable());
        }

        public async Task<IEnumerable<TaskWithUserName>> GetTasksWithUserNamesAsync()
        {
            return await ThreadingTask.Run(() =>
            {
                var result = from task in _dbContext.Tasks
                             join user in _dbContext.Users on task.UserId equals user.Id into userTasks
                             from user in userTasks.DefaultIfEmpty()  // DefaultIfEmpty handles the left join
                             select new TaskWithUserName
                             {
                                 Id = task.Id,
                                 UserId = task.UserId,
                                 Description = task.Description,
                                 DueDate = task.DueDate,
                                 UserName = (user != null) ? user.Name : null ,
                                 Completed = task.Completed
                             };

                return result.ToList();
            });
        }

        public async Task<ModelTask> AddTaskAsync(ModelTask task)
        {
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return await ThreadingTask.Run(() => task);
        }

        public async Task<bool> IsTaskWithGivenDescriptionExist(ModelTask task)
        {
            return await ThreadingTask.Run(() => _dbContext.Tasks.
                                Where(t => t.UserId == task.UserId && t.Description == task.Description).Count() > 0);
        }

        public async Task<IEnumerable<ModelTask>> GetOverdueTasksAsync(DateTime date)
        {
            return await ThreadingTask.Run(() => _dbContext.Tasks.
                                                Where(t=> t.DueDate.Date > date.Date).
                                                AsEnumerable());
        }

        public async Task<bool> UserHasLessThan10TasksAsync(int userId)
        {
            int count = _dbContext.Tasks.Where(t => t.Completed == false && t.UserId == userId).Count();
            return await ThreadingTask.Run(() => _dbContext.Tasks.Where(t => t.Completed == false && t.UserId == userId).Count() < 10);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using UserTasksBackend.Models;
using ModelTask = UserTasksBackend.Models.Task;

namespace UserTasksBackend.Services
{
    public interface ITaskService
    {
        Task<ModelTask> GetTaskAsync(int taskId);
        Task<IEnumerable<ModelTask>> GetTasksAsync();
        Task<ModelTask> AddTaskAsync(ModelTask task);
        Task<IEnumerable<ModelTask>> GetOverdueTasksAsync(DateTime date);
        Task<bool> UserHasLessThan10TasksAsync(int userId);
        Task<IEnumerable<TaskWithUserName>> GetTasksWithUserNamesAsync();
        Task<bool> IsTaskWithGivenDescriptionExist(ModelTask task);
    }
}

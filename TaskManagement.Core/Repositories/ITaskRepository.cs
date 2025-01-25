using TaskManagement.Core.Entities;

namespace TaskManagement.Core.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskEntity>> GetTasksCompletedInLast30DaysAsync();
    }
}

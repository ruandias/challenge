using TaskManagement.Core.DTOs.Task;
using TaskManagement.Core.Responses;

namespace TaskManagement.Core.Contracts
{
    public interface ITaskService
    {
        Task<AppResponse<IEnumerable<TaskDto>>> GetTasksByProjectAsync(Guid projectId);
        Task<AppResponse<TaskDto>> CreateTaskAsync(CreateTaskDto task);
        Task<AppResponse<string>> UpdateTaskAsync(Guid taskId, string status, string description, Guid userId);
        Task<AppResponse<string>> DeleteTaskAsync(Guid taskId);
    }
}

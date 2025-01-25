using TaskManagement.Core.DTOs.Report;

namespace TaskManagement.Core.Repositories
{
    public interface ITaskRepository
    {
        Task<List<UserTaskPerformanceDto>> GetUserTaskPerformanceReportAsync();
    }
}

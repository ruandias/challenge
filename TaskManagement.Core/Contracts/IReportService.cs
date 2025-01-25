using TaskManagement.Core.DTOs.Report;
using TaskManagement.Core.Responses;

namespace TaskManagement.Core.Contracts
{
    public interface IReportService
    {
        Task<AppResponse<List<UserTaskPerformanceDto>>> GetAverageTasksCompletedAsync(string role);
    }
}

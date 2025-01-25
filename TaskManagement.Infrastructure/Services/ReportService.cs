using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Report;
using TaskManagement.Core.Repositories;
using TaskManagement.Core.Responses;

namespace TaskManagement.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly ITaskRepository _taskRepository;

        public ReportService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<AppResponse<List<UserTaskPerformanceDto>>> GetAverageTasksCompletedAsync(string role)
        {
            if (role.ToLower() != "manager")
            {
                return new AppResponse<List<UserTaskPerformanceDto>> 
                {
                    Data = null,
                    Errors = new List<string> { "Access denied. Only users with the 'manager' role can access this endpoint." },
                    Message = "Forbidden",
                    StatusCode = 403,
                    Success = false
                };
            }

            var averageTasksByUser = await _taskRepository.GetUserTaskPerformanceReportAsync();

            var response = new AppResponse<List<UserTaskPerformanceDto>>()
            {
                Data = averageTasksByUser,
                Errors = null,
                Message = "Ok",
                StatusCode = 200,
                Success = true
            };

            return response;
        }
    }
}

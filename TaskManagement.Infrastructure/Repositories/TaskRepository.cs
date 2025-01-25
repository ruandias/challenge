using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.DTOs.Report;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Repositories;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserTaskPerformanceDto>> GetUserTaskPerformanceReportAsync()
        {
            var last30Days = DateTime.UtcNow.AddDays(-30);

            var completedTasks = await _context.Tasks
                .Include(task => task.Project)
                .Where(task => task.Status == "Completed" && task.DueDate >= last30Days)
                .ToListAsync();

            var averageTasksByUser = completedTasks
              .Where(task => task.Project != null)
              .GroupBy(task => task.Project.UserId)
              .Select(group => new UserTaskPerformanceDto
              {
                  UserId = group.Key,
                  AverageTasksCompleted = group.Count()
              })
              .ToList();

            return averageTasksByUser;
        }
    }
}

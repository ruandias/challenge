using Microsoft.EntityFrameworkCore;
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

        public async Task<List<TaskEntity>> GetTasksCompletedInLast30DaysAsync()
        {
            var last30Days = DateTime.UtcNow.AddDays(-30);

            return await _context.Tasks
                .Where(task => task.Status == "Completed" && task.DueDate >= last30Days)
                .ToListAsync();
        }
    }
}

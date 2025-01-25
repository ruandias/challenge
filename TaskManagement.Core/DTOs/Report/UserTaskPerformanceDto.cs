namespace TaskManagement.Core.DTOs.Report
{
    public class UserTaskPerformanceDto
    {
        public Guid UserId { get; set; }
        public int AverageTasksCompleted { get; set; }
    }
}

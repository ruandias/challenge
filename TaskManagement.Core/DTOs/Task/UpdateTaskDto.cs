namespace TaskManagement.Core.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public Guid UserId  { get; set; }
    }
}

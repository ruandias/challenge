namespace TaskManagement.Core.DTOs.Project
{
    public class CreateProjectTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } // Ex: "Low", "Medium", "High"
    }
}

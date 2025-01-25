namespace TaskManagement.Core.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } // Low, Medium, High
        public Guid ProjectId { get; set; } // Relacionamento com o Projeto
        public string InitialComment { get; set; }
        public Guid UserId { get; set; }
    }
}

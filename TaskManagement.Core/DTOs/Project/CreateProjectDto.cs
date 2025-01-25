namespace TaskManagement.Core.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }

        public List<CreateProjectTaskDto> Tasks { get; set; } = new();
    }
}

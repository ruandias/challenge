namespace TaskManagement.Core.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // Pending, InProgress, Completed
        public string Priority { get; set; } // Low, Medium, High
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
        public ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public ICollection<TaskHistoryEntity> Histories { get; set; } = new List<TaskHistoryEntity>();
    }
}

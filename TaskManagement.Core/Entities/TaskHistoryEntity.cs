namespace TaskManagement.Core.Entities
{
    public class TaskHistoryEntity
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public TaskEntity Task { get; set; }
        public string ChangeDescription { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid UserId { get; set; }
    }
}

namespace TaskManagement.Core.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    }
}

namespace TaskManagement.Core.DTOs.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}

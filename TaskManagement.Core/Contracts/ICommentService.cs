using TaskManagement.Core.DTOs.Comment;
using TaskManagement.Core.Responses;

namespace TaskManagement.Core.Contracts
{
    public interface ICommentService
    {
        Task<AppResponse<CommentDto>> AddCommentAsync(Guid taskId, string content, Guid userId);
        Task<AppResponse<string>> UpdateCommentAsync(Guid commentId, string content);
        Task<AppResponse<string>> DeleteCommentAsync(Guid commentId);
    }
}

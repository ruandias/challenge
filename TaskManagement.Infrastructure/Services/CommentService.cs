using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Comment;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Responses;

namespace TaskManagement.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<TaskEntity> _taskRepository;
        private readonly IRepository<CommentEntity> _commentRepository;

        public CommentService(
            IRepository<TaskEntity> taskRepository, 
            IRepository<CommentEntity> commentRepository)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
        }

        public async Task<AppResponse<CommentDto>> AddCommentAsync(Guid taskId, string content, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null)
            {
                return new AppResponse<CommentDto>
                {
                    Data = null,
                    Errors = new List<string> { "Not Found" },
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }
                
            var comment = new CommentEntity
            {
                Id = Guid.NewGuid(),
                Content = content,
                TaskId = taskId,
                CreatedDate = DateTime.UtcNow,
                UserId = userId
            };

            await _commentRepository.AddAsync(comment);

            task.Histories.Add(new TaskHistoryEntity
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                ChangeDescription = content,
                ChangeDate = DateTime.UtcNow,
                UserId = userId
            });

            await _taskRepository.UpdateAsync(task);


            var commentDto = new CommentDto()
            {
                Content = content
            };

            var response = new AppResponse<CommentDto>
            {
                Data = commentDto,
                Errors = new List<string> { },
                Message = "Created",
                StatusCode = 201,
                Success = true
            };

            return response;
        }

        public async Task<AppResponse<string>> UpdateCommentAsync(Guid commentId, string content)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                return new AppResponse<string>
                {
                    Data = null,
                    Errors = new List<string> { "Not found" },
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }


            comment.Content = content;
            await _commentRepository.UpdateAsync(comment);

            var response = new AppResponse<string>
            {
                Data = null,
                Errors = null,
                Success = true,
                Message = "NoContent",
                StatusCode = 204
            };

            return response;
        }

        public async Task<AppResponse<string>> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                return new AppResponse<string>
                {
                    Data = null,
                    Errors = new List<string> { "Not found" },
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }

            await _commentRepository.DeleteAsync(comment);

            var response = new AppResponse<string>
            {
                Data = null,
                Errors = null,
                Success = true,
                Message = "NoContent",
                StatusCode = 204
            };

            return response;
        }
    }

}

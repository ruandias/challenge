using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Comment;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("{taskId}")]
        public async Task<IActionResult> AddComment(Guid taskId, [FromBody] CreateCommentDto commentDto)
        {
            try
            {
                await _commentService.AddCommentAsync(taskId, commentDto.Content, commentDto.UserId);
                return Ok(new { message = "Comentário adicionado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentDto commentDto)
        {
            try
            {
                await _commentService.UpdateCommentAsync(commentId, commentDto.Content);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            try
            {
                await _commentService.DeleteCommentAsync(commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

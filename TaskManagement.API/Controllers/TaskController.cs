using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Task;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetTasksByProject(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(dto);
                return CreatedAtAction(nameof(GetTasksByProject), new { projectId = dto.ProjectId }, createdTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskDto UpdateTaskDto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(
                    taskId, 
                    UpdateTaskDto.Status, 
                    UpdateTaskDto.Description, 
                    UpdateTaskDto.UserId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            try
            {
                await _taskService.DeleteTaskAsync(taskId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

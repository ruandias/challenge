using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Project;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProjectsByUser(Guid userId)
        {
            var projects = await _projectService.GetAllProjectsByUserAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto project)
        {
            var createdProject = await _projectService.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProjectsByUser), new { userId = project.UserId }, createdProject);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            try
            {
                await _projectService.DeleteProjectAsync(projectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

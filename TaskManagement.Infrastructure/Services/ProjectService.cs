using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Project;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Responses;

namespace TaskManagement.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<ProjectEntity> _projectRepository;

        public ProjectService(
            IRepository<ProjectEntity> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<AppResponse<IEnumerable<ProjectDto>>> GetAllProjectsByUserAsync(Guid userId)
        {
            var projectEntities = await _projectRepository.FindAsync(p => p.UserId == userId);

            if (projectEntities == null)
            {
                return new AppResponse<IEnumerable<ProjectDto>>
                {
                    Data = null,
                    Errors = new List<string>() { "Not Found" },
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }

            var projectDtos = projectEntities.Select(p => new ProjectDto
            {
                Name = p.Name
            });

            var response = new AppResponse<IEnumerable<ProjectDto>>()
            {
                Data = projectDtos,
                Errors = null,
                Message = "Ok",
                StatusCode = 200,
                Success = true
            };

            return response;
        }

        public async Task<AppResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto projectDto)
        {
            var projectEntity = new ProjectEntity
            {
                Id = Guid.NewGuid(),
                Name = projectDto.Name,
                UserId = projectDto.UserId,
                Tasks = projectDto.Tasks?.Select(taskDto => new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    DueDate = taskDto.DueDate ?? DateTime.UtcNow.AddDays(7),
                    Priority = taskDto.Priority,
                    Status = "Pending"
                }).ToList() ?? new List<TaskEntity>()
            };

            await _projectRepository.AddAsync(projectEntity);
            await _projectRepository.SaveChangesAsync();

            var dto = new ProjectDto()
            {
                Name = projectEntity.Name
            };

            var response = new AppResponse<ProjectDto>()
            {
                Data = dto,
                Message = "Created",
                Errors = null,
                StatusCode = 201,
                Success = true
            };

            return response;
        }

        public async Task<AppResponse<string>> DeleteProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
            {
                new AppResponse<string>
                {
                    Success = true,
                    Data = null,
                    Errors = new List<string>() { "Project not found"},
                    Message = "NotFound",
                    StatusCode = 404
                };
            }

            if (project.Tasks.Any(t => t.Status == "Pending"))
            {
                new AppResponse<string>
                {
                    Success = true,
                    Data = null,
                    Errors = new List<string>() { "Cannot delete a project with pending tasks." },
                    Message = "InternalServerError",
                    StatusCode = 500
                };
            }

            await _projectRepository.DeleteAsync(project);
            await _projectRepository.SaveChangesAsync();

            var response = new AppResponse<string>
            {
                Success = true,
                Data = null,
                Errors = null,
                Message = "NoContent",
                StatusCode = 204
            };

            return response;
        }
    }
}

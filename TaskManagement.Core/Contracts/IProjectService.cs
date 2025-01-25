using TaskManagement.Core.DTOs.Project;
using TaskManagement.Core.Responses;

namespace TaskManagement.Core.Contracts
{
    public interface IProjectService
    {
        Task<AppResponse<IEnumerable<ProjectDto>>> GetAllProjectsByUserAsync(Guid userId);
        Task<AppResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto projectDto);
        Task<AppResponse<string>> DeleteProjectAsync(Guid projectId);
    }
}

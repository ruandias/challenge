using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Task;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Enums;
using TaskManagement.Core.Responses;

namespace TaskManagement.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskEntity> _taskRepository;
        private readonly IRepository<ProjectEntity> _projectRepository;
        private readonly IRepository<TaskHistoryEntity> _historyRepository;
        private readonly IRepository<CommentEntity> _commentRepository;

        public TaskService(
            IRepository<TaskEntity> taskRepository,
            IRepository<ProjectEntity> projectRepository,
            IRepository<TaskHistoryEntity> historyRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _historyRepository = historyRepository;
        }

        public async Task<AppResponse<IEnumerable<TaskDto>>> GetTasksByProjectAsync(Guid projectId)
        {
            var tasksEntiies =  await _taskRepository.FindAsync(t => t.ProjectId == projectId);

            if (tasksEntiies == null)
            {
                return new AppResponse<IEnumerable<TaskDto>> 
                {
                    Data = null, 
                    Errors = new List<string>() { "Task not found"},
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }

            var dtos = tasksEntiies.Select(t => new TaskDto
            {
                Title = t.Title
            });


            var response = new AppResponse<IEnumerable<TaskDto>>()
            {
                Data = dtos,
                Errors = null,
                Message = "Ok",
                StatusCode = 200,
                Success = true
            };

            return response;

        }

        public async Task<AppResponse<TaskDto>> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var project = await _projectRepository.GetByIdAsync(createTaskDto.ProjectId);

            if (project == null)
            {
                return new AppResponse<TaskDto>()
                {
                    Data = null,
                    Errors = new List<string>() { "Project not found" },
                    Message = "Not found",
                    StatusCode = 404,
                    Success = false
                };
            }


            if (project.Tasks.Count >= 20)
            {
                return new AppResponse<TaskDto>()
                {
                    Data = null,
                    Errors = new List<string>() { "A project cannot have more than 20 tasks." },
                    Message = "Conflict",
                    StatusCode = 409,
                    Success = false
                };
            }


            var taskEntity = new TaskEntity()
            {
                Id = Guid.NewGuid(),
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Status = EStatus.Pending.ToString(),
                Priority = createTaskDto.Priority,
                ProjectId = createTaskDto.ProjectId,
            };

            await _taskRepository.AddAsync(taskEntity);

            if (!string.IsNullOrEmpty(createTaskDto.InitialComment))
            {
                var comment = new CommentEntity()
                {
                    Id = Guid.NewGuid(),
                    Content = createTaskDto.InitialComment,
                    TaskId = taskEntity.Id,
                    CreatedDate = DateTime.UtcNow,
                    UserId = createTaskDto.UserId
                };

                await _commentRepository.AddAsync(comment);
            }

            await _taskRepository.SaveChangesAsync();

            var taskDto = new TaskDto()
            {
                Title = taskEntity.Title
            };

            var response = new AppResponse<TaskDto>()
            {
                Data = taskDto,
                Errors = null,
                Message = "Created",
                StatusCode = 201,
                Success = false
            };

            return response;
        }

        public async Task<AppResponse<string>> UpdateTaskAsync(Guid taskId, string status, string description, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null)
            {
                return new AppResponse<string>()
                {
                    Data = null,
                    Errors = new List<string>() { "Not Found" },
                    Success = false,
                    Message = "NotFound",
                    StatusCode = 404
                };
            }

            var history = new TaskHistoryEntity
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                ChangeDescription = status,
                ChangeDate = DateTime.Now,
                UserId  = userId
            };

            task.Status = status;
            task.Description = description;

            await _historyRepository.AddAsync(history);
            await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();

            var response = new AppResponse<string>()
            {
                Data = null,
                Errors = null,
                Message = "NoContent",
                StatusCode = 204,
                Success = true
            };

            return response;
        }

        public async Task<AppResponse<string>> DeleteTaskAsync(Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null)
            {
                return new AppResponse<string>()
                {
                    Data = null, 
                    Errors = new List<string>() { "Not found"},
                    Message = "NotFound",
                    StatusCode = 404,
                    Success = false
                };
            }

            await _taskRepository.DeleteAsync(task);
            await _taskRepository.SaveChangesAsync();

            var response = new AppResponse<string>()
            {
                Data = null,
                Errors = null,
                Message = "NoContet",
                StatusCode = 204,
                Success = true
            };

            return response;
        }
    }
}

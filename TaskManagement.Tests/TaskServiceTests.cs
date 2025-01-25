using Moq;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Task;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IRepository<TaskEntity>> _taskRepositoryMock;
        private readonly Mock<IRepository<ProjectEntity>> _projectRepositoryMock;
        private readonly Mock<IRepository<TaskHistoryEntity>> _historyRepositoryMock;
        private readonly Mock<IRepository<CommentEntity>> _commentRepositoryMock;

        private readonly TaskService _underTest;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<IRepository<TaskEntity>>();
            _projectRepositoryMock = new Mock<IRepository<ProjectEntity>>();
            _historyRepositoryMock = new Mock<IRepository<TaskHistoryEntity>>();
            _commentRepositoryMock = new Mock<IRepository<CommentEntity>>();

            _underTest = new TaskService(
                _taskRepositoryMock.Object,
                _projectRepositoryMock.Object,
                _historyRepositoryMock.Object,
                _commentRepositoryMock.Object
                );
        }

        #region CreateTaskAsync Tests

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { ProjectId = Guid.NewGuid() };
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(createTaskDto.ProjectId)).ReturnsAsync((ProjectEntity)null);

            // Act
            var result = await _underTest.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Project not found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnConflict_WhenProjectHasTooManyTasks()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { ProjectId = Guid.NewGuid() };
            var project = new ProjectEntity { Id = createTaskDto.ProjectId, Tasks = new List<TaskEntity>(new TaskEntity[20]) };
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(createTaskDto.ProjectId)).ReturnsAsync(project);

            // Act
            var result = await _underTest.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(409, result.StatusCode);
            Assert.Contains("A project cannot have more than 20 tasks.", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnCreated_WhenTaskIsCreatedSuccessfully()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                ProjectId = Guid.NewGuid(),
                Title = "New Task",
                Description = "Task Description",
                Priority = "High",
                DueDate = DateTime.UtcNow,
                InitialComment = "Initial Comment",
                UserId = Guid.NewGuid()
            };

            var project = new ProjectEntity { Id = createTaskDto.ProjectId, Tasks = new List<TaskEntity>() };
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(createTaskDto.ProjectId)).ReturnsAsync(project);
            _taskRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TaskEntity>())).Returns(Task.CompletedTask);
            _commentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CommentEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(createTaskDto.Title, result.Data.Title);
            _taskRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
        }

        #endregion

        #region UpdateTaskAsync Tests

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var status = "In Progress";
            var description = "Task updated description.";
            var userId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _underTest.UpdateTaskAsync(taskId, status, description, userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not Found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnNoContent_WhenTaskIsUpdatedSuccessfully()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var status = "In Progress";
            var description = "Task updated description.";
            var userId = Guid.NewGuid();

            var taskEntity = new TaskEntity { Id = taskId, Status = "Pending", Description = "Old description" };
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _historyRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TaskHistoryEntity>())).Returns(Task.CompletedTask);
            _taskRepositoryMock.Setup(repo => repo.UpdateAsync(taskEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.UpdateTaskAsync(taskId, status, description, userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Null(result.Errors);
            Assert.Equal(status, taskEntity.Status);
            Assert.Equal(description, taskEntity.Description);
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(taskEntity), Times.Once);
        }

        #endregion

        #region DeleteTaskAsync Tests

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _underTest.DeleteTaskAsync(taskId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnNoContent_WhenTaskIsDeletedSuccessfully()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var taskEntity = new TaskEntity { Id = taskId };
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _taskRepositoryMock.Setup(repo => repo.DeleteAsync(taskEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.DeleteTaskAsync(taskId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(result.Errors);
            Assert.Null(result.Data);
            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(taskEntity), Times.Once);
        }

        #endregion
    }


}

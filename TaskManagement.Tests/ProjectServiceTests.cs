using Moq;
using System.Linq.Expressions;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.DTOs.Project;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IRepository<ProjectEntity>> _projectRepositoryMock;
        private readonly ProjectService _underTest;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IRepository<ProjectEntity>>();

            _underTest = new ProjectService(_projectRepositoryMock.Object);
        }

        #region GetAllProjectsByUserAsync Tests

        [Fact]
        public async Task GetAllProjectsByUserAsync_ShouldReturnNotFound_WhenNoProjectsExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _projectRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<ProjectEntity, bool>>>()))
                .ReturnsAsync((IEnumerable<ProjectEntity>)null);

            // Act
            var result = await _underTest.GetAllProjectsByUserAsync(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not Found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAllProjectsByUserAsync_ShouldReturnProjects_WhenProjectsExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectEntities = new List<ProjectEntity>
        {
            new ProjectEntity { Name = "Project 1", UserId = userId },
            new ProjectEntity { Name = "Project 2", UserId = userId }
        };

            _projectRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<ProjectEntity, bool>>>()))
                .ReturnsAsync(projectEntities);

            // Act
            var result = await _underTest.GetAllProjectsByUserAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
            Assert.Contains(result.Data, p => p.Name == "Project 1");
            Assert.Contains(result.Data, p => p.Name == "Project 2");
        }

        #endregion

        #region CreateProjectAsync Tests

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnCreated_WhenProjectIsCreatedSuccessfully()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = "New Project",
                UserId = Guid.NewGuid(),
                Tasks = null
            };

            _projectRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ProjectEntity>())).Returns(Task.CompletedTask);
            _projectRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.CreateProjectAsync(createProjectDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal("New Project", result.Data.Name);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnBadRequest_WhenProjectCannotBeCreated()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = "",
                UserId = Guid.NewGuid()
            };

            // Act
            var result = await _underTest.CreateProjectAsync(createProjectDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Contains("Invalid project name", result.Errors);
        }

        #endregion

        #region DeleteProjectAsync Tests

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((ProjectEntity)null);

            // Act
            var result = await _underTest.DeleteProjectAsync(projectId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Project not found", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnInternalServerError_WhenProjectHasPendingTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectEntity = new ProjectEntity
            {
                Id = projectId,
                Tasks = new List<TaskEntity>
                {
                    new TaskEntity { Status = "Pending" }
                }
            };

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projectEntity);

            // Act
            var result = await _underTest.DeleteProjectAsync(projectId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Cannot delete a project with pending tasks", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnNoContent_WhenProjectIsDeletedSuccessfully()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectEntity = new ProjectEntity
            {
                Id = projectId,
                Tasks = new List<TaskEntity>
                {
                    new TaskEntity { Status = "Completed" }
                }
            };

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projectEntity);
            _projectRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<ProjectEntity>())).Returns(Task.CompletedTask);
            _projectRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.DeleteProjectAsync(projectId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(result.Errors);
        }

        #endregion
    }

}

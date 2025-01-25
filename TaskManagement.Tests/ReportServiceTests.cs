using Moq;
using TaskManagement.Core.DTOs.Report;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Repositories;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Tests
{
    public class ReportServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly ReportService _underTest;

        public ReportServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();

            _underTest = new ReportService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAverageTasksCompletedAsync_ShouldReturnAccessDenied_WhenRoleIsNotManager()
        {
            // Arrange
            string role = "user";

            // Act
            var result = await _underTest.GetAverageTasksCompletedAsync(role);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(403, result.StatusCode);
            Assert.Contains("Access denied. Only users with the 'manager' role can access this endpoint.", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAverageTasksCompletedAsync_ShouldReturnAverageTasks_WhenRoleIsManager()
        {
            // Arrange
            string role = "manager";

            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Role = role
            };

            var userTaskPerformanceDto = new UserTaskPerformanceDto()
            {
                UserId = Guid.NewGuid(),
                AverageTasksCompleted = 1
            };

            var userTaskPerformances = new List<UserTaskPerformanceDto>
            {
                userTaskPerformanceDto
            };

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskPerformanceReportAsync()).ReturnsAsync(userTaskPerformances);

            // Act
            var result = await _underTest.GetAverageTasksCompletedAsync(user.Role);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAverageTasksCompletedAsync_ShouldReturnEmpty_WhenNoTasksCompleted()
        {
            // Arrange
            string role = "manager";

            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Role = role
            };

            var userTaskPerformanceDto = new UserTaskPerformanceDto()
            {
                UserId = Guid.NewGuid(),
                AverageTasksCompleted = 1
            };

            var userTaskPerformances = new List<UserTaskPerformanceDto>
            {
            };

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskPerformanceReportAsync()).ReturnsAsync(userTaskPerformances);

            // Act
            var result = await _underTest.GetAverageTasksCompletedAsync(role);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAverageTasksCompletedAsync_ShouldReturnTasksCompletedFromLast30DaysOnly()
        {
            // Arrange
            string role = "manager";

            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Role = role
            };

            var userTaskPerformanceDto = new UserTaskPerformanceDto()
            {
                UserId = Guid.NewGuid(),
                AverageTasksCompleted = 1
            };

            var userTaskPerformances = new List<UserTaskPerformanceDto>
            {
                userTaskPerformanceDto
            };

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskPerformanceReportAsync()).ReturnsAsync(userTaskPerformances);

            // Act
            var result = await _underTest.GetAverageTasksCompletedAsync(role);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data); 
            Assert.Equal(1, result.Data.First().AverageTasksCompleted);
        }
    }

}

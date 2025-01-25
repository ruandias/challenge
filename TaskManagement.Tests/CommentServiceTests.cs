using Moq;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<IRepository<TaskEntity>> _taskRepositoryMock;
        private readonly Mock<IRepository<CommentEntity>> _commentRepositoryMock;
        private readonly Mock<IRepository<TaskHistoryEntity>> _taskHistoryRepositoryMock;
        private readonly CommentService _underTest;

        public CommentServiceTests()
        {
            _taskRepositoryMock = new Mock<IRepository<TaskEntity>>();
            _commentRepositoryMock = new Mock<IRepository<CommentEntity>>();
            _taskHistoryRepositoryMock = new Mock<IRepository<TaskHistoryEntity>>();

            _underTest = new CommentService(
                _taskRepositoryMock.Object, 
                _commentRepositoryMock.Object,
                _taskHistoryRepositoryMock.Object
                );
        }

        #region AddCommentAsync Tests

        [Fact]
        public async Task AddCommentAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var content = "This is a comment.";
            var userId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _underTest.AddCommentAsync(taskId, content, userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not Found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddCommentAsync_ShouldReturnCreated_WhenCommentIsAddedSuccessfully()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var content = "This is a comment.";
            var userId = Guid.NewGuid();

            var taskEntity = new TaskEntity { Id = taskId, Histories = new List<TaskHistoryEntity>() };
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _commentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CommentEntity>())).Returns(Task.CompletedTask);
            _taskRepositoryMock.Setup(repo => repo.UpdateAsync(taskEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.AddCommentAsync(taskId, content, userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(content, result.Data.Content);
            _commentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CommentEntity>()), Times.Once);
            //_taskRepositoryMock.Verify(repo => repo.UpdateAsync(taskEntity), Times.Once);
        }

        #endregion

        #region UpdateCommentAsync Tests

        [Fact]
        public async Task UpdateCommentAsync_ShouldReturnNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var newContent = "Updated comment content.";

            _commentRepositoryMock.Setup(repo => repo.GetByIdAsync(commentId)).ReturnsAsync((CommentEntity)null);

            // Act
            var result = await _underTest.UpdateCommentAsync(commentId, newContent);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateCommentAsync_ShouldReturnNoContent_WhenCommentIsUpdatedSuccessfully()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var newContent = "Updated comment content.";

            var commentEntity = new CommentEntity { Id = commentId, Content = "Original content." };
            _commentRepositoryMock.Setup(repo => repo.GetByIdAsync(commentId)).ReturnsAsync(commentEntity);
            _commentRepositoryMock.Setup(repo => repo.UpdateAsync(commentEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.UpdateCommentAsync(commentId, newContent);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Null(result.Errors);
            Assert.Equal(newContent, commentEntity.Content);
            _commentRepositoryMock.Verify(repo => repo.UpdateAsync(commentEntity), Times.Once);
        }

        #endregion

        #region DeleteCommentAsync Tests

        [Fact]
        public async Task DeleteCommentAsync_ShouldReturnNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            var commentId = Guid.NewGuid();

            _commentRepositoryMock.Setup(repo => repo.GetByIdAsync(commentId)).ReturnsAsync((CommentEntity)null);

            // Act
            var result = await _underTest.DeleteCommentAsync(commentId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Not found", result.Errors);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteCommentAsync_ShouldReturnNoContent_WhenCommentIsDeletedSuccessfully()
        {
            // Arrange
            var commentId = Guid.NewGuid();

            var commentEntity = new CommentEntity { Id = commentId, Content = "This is a comment." };
            _commentRepositoryMock.Setup(repo => repo.GetByIdAsync(commentId)).ReturnsAsync(commentEntity);
            _commentRepositoryMock.Setup(repo => repo.DeleteAsync(commentEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await _underTest.DeleteCommentAsync(commentId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(result.Errors);
            Assert.Null(result.Data);
            _commentRepositoryMock.Verify(repo => repo.DeleteAsync(commentEntity), Times.Once);
        }

        #endregion
    }

}

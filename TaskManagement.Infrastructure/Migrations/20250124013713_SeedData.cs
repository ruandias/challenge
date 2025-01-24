using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                { user1Id, "Admin User" },
                { user2Id, "John Doe" }
                });

            var projects1Id = Guid.NewGuid();
            var projects2Id = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                { projects1Id, "Project Alpha", user1Id },
                { projects2Id, "Project Beta", user2Id }
                });

            var task1Id = Guid.NewGuid();
            var task2Id = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Title", "Description", "DueDate", "Status", "Priority", "ProjectId" },
                values: new object[,]
                {
                { task1Id, "Task 1", "Description for Task 1", DateTime.UtcNow.AddDays(7), "Pending", "High", projects1Id }, 
                { task2Id, "Task 2", "Description for Task 2", DateTime.UtcNow.AddDays(5), "InProgress", "Medium", projects2Id } 
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "TaskId", "CreatedDate", "UserId" },
                values: new object[,]
                {
                { Guid.NewGuid(), "Initial comment for Task 1", task1Id, DateTime.UtcNow, user1Id },
                { Guid.NewGuid(), "Initial comment for Task 2", task2Id, DateTime.UtcNow, user2Id }
                });

            migrationBuilder.InsertData(
                table: "TaskHistories",
                columns: new[] { "Id", "TaskId", "ChangeDescription", "ChangeDate", "UserId" },
                values: new object[,]
                {
                { Guid.NewGuid(), task1Id, "Task created", DateTime.UtcNow, user1Id },
                { Guid.NewGuid(), task2Id, "Task in progress", DateTime.UtcNow, user2Id }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Users");

            migrationBuilder.Sql("DELETE FROM Projects");

            migrationBuilder.Sql("DELETE FROM Tasks");

            migrationBuilder.Sql("DELETE FROM Comments");

            migrationBuilder.Sql("DELETE FROM TaskHistories");
        }
    }
}

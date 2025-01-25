
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Extensions;
using TaskManagement.Core.Contracts;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Repositories;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IRepository<TaskHistoryEntity>, Repository<TaskHistoryEntity>>();
            builder.Services.AddScoped<IRepository<CommentEntity>, Repository<CommentEntity>>();
            builder.Services.AddScoped<IRepository<TaskEntity>, Repository<TaskEntity>>();
            builder.Services.AddScoped<IRepository<ProjectEntity>, Repository<ProjectEntity>>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IReportService,  ReportService>();
            builder.Services.AddScoped<ITaskService, TaskService>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ApplyMigrations();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

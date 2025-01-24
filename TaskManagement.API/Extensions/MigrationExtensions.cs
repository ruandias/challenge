using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var retryCount = 5;
            var delay = TimeSpan.FromSeconds(5);

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    return;
                }
                catch (Exception ex) when (i < retryCount - 1)
                {
                    Console.WriteLine($"Tentativa {i + 1} falhou. Re-tentando em {delay.Seconds} segundos...");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Não foi possível conectar ao banco de dados após várias tentativas.");
        }
    }
    

}

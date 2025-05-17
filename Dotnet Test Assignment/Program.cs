
using Dotnet_Test_Assignment.Controllers;
using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Repostories;
using Dotnet_Test_Assignment.Services;
namespace Dotnet_Test_Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMemoryCache();
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
            builder.Services.AddSingleton<ILogRepository, LogRepository>();
            builder.Services.AddHttpClient<IIpLookupService, IpLookupService>();
            builder.Services.AddSingleton<ITemporaryBlockService, TemporaryBlockService>();
            builder.Services.AddHostedService<TemporaryBlockCleanupService>();
            builder.Services.AddHttpContextAccessor();

            //builder.Services.AddHostedService<TemporalBlockCleanupService>();


            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //Adding Logging meddilware
            app.UseMiddleware<LoggingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

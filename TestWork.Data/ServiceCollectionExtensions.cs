using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestWork.Data.Context;
using TestWork.Data.Repositories;
using TestWork.Repositories;

namespace TestWork.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(x =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder
                    .UseNpgsql(connectionString);

                return optionsBuilder;
            });

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IProjectsRepository, ProjectsRepository>();
            services.AddTransient<IProjectTasksRepository, ProjectTasksRepository>();
            services.AddTransient<IProjectStagesRepository, ProjectStagesRepository>();

            return services;
        }
    }
}

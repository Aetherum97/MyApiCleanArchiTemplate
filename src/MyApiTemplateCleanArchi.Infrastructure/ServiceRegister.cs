using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Bases;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Interfaces.Repositories;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using System.Reflection;

namespace MyApiTemplateCleanArchi.Infrastructure
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(BaseRepository<>));

            var repositoryTypes = assembly!.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t =>
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>)));

            foreach (var implementation in repositoryTypes)
            {
                var interfaces = implementation.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>));
                foreach (var serviceType in interfaces)
                {
                    services.AddScoped(serviceType, implementation);
                }
            }

            return services;
        }

        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            

            services.RegisterRepositories();

            return services;
        }
    }
}
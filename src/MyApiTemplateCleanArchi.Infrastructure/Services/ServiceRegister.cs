using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Bases;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Interfaces.Repositories;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using System.Reflection;

namespace MyApiTemplateCleanArchi.Infrastructure.Services
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            var baseRepoType = typeof(BaseRepository<,>);
            var assembly = baseRepoType.Assembly;

            var implementations = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.BaseType != null
                     && t.BaseType.IsGenericType
                     && t.BaseType.GetGenericTypeDefinition() == baseRepoType);

            foreach (var impl in implementations)
            {
                var serviceInterface = impl.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IBaseRepository<>));

                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, impl);
                }
            }

            return services;
        }

        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext")));
            

            services.RegisterRepositories();

            return services;
        }
    }
}
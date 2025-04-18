using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MyApiTemplateCleanArchi.Infrastructure.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var contexts = assembly.GetTypes()
                .Where(t =>  !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t));

            foreach (var contextType in contexts)
            {
                var name = contextType.Name;
                var connectionString = configuration.GetConnectionString(name) ?? throw new InvalidOperationException($"No connection string found for : {name}");

                var method = typeof(EntityFrameworkServiceCollectionExtensions)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == "AddDbContext" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(contextType);

                Action<IServiceProvider, DbContextOptionsBuilder> configure = (serviceProvider, options) =>
                {
                    if (name.EndsWith("PostgreDbContext", StringComparison.OrdinalIgnoreCase))
                    {
                        options.UseNpgsql(connectionString, database => database.MigrationsAssembly(assembly.GetName().Name));
                    }
                    else 
                    {
                        options.UseSqlServer(connectionString, database => database.MigrationsAssembly(assembly.GetName().Name));
                    }
                    // Add another db in else if conditional structure...
                };

                method.Invoke(null, new object[] { services, configure });
            }

            return services;
        }
    }
}
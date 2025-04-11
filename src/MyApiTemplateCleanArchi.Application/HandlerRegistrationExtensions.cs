using Microsoft.Extensions.DependencyInjection;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using System.Reflection;

namespace MyApiTemplateCleanArchi.Application
{
    public static class HandlerRegistrationExtensions
    {
        public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IRequestHandler<,>);

            var registrations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .Select(i => new { ServiceType = i, ImplementationType = t }))
                .ToList();

            foreach (var reg in registrations)
            {
                services.AddTransient(reg.ServiceType, reg.ImplementationType);
            }

            return services;
        }
    }
}
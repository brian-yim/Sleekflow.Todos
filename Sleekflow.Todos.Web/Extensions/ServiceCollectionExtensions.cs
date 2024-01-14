using Sleekflow.Todos.Core.Services;

namespace Sleekflow.Todos.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }

}
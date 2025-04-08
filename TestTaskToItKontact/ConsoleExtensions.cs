using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestTaskToItKontact;

internal static class ConsoleExtensions
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services
            .AddLogging(x => x.AddConsole());

        return services;
    }
}
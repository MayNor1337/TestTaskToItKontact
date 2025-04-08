using Microsoft.Extensions.DependencyInjection;
using TestTaskToItKontact.Domain.Helpers;
using TestTaskToItKontact.Domain.Services;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace TestTaskToItKontact.Domain.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddTransient<IFileHelper, FileHelper>()
            .AddTransient<ICsvService, CsvService>()
            .AddTransient<IExelService, ExelService>()
            .AddTransient<IScenarioStrategy, StandardScenario>();

        return services;
    }
}
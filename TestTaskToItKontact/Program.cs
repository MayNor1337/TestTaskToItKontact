using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTaskToItKontact.Data.Extensions;
using TestTaskToItKontact.Domain.Extensions;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace TestTaskToItKontact;

public class Program
{
    private static IServiceProvider _serviceProvider;
    private static ILogger _logger;

    public static async Task Main(string[] args)
    {
        CreateServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        UpMigration(scope);

        _logger = scope.ServiceProvider.GetService<ILogger<Program>>();

        _logger.LogInformation("Starting up");

        var scenarioStrategy = scope.ServiceProvider.GetService<IScenarioStrategy>();

        try
        {
            await scenarioStrategy.HandleScenario(args);
        }
        catch (Exception e)
        {
            Console.WriteLine("Произошла ошибка, попробуйте в другой раз");
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private static void UpMigration(IServiceScope scope)
    {
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        migrator.MigrateUp();
    }

    private static void CreateServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _serviceProvider = new ServiceCollection()
            .AddConsoleServices()
            .AddDataServices(configuration)
            .AddDomainServices()
            .BuildServiceProvider();
    }
}
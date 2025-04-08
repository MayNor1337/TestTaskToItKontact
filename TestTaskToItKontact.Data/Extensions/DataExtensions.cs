using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TestTaskToItKontact.Data.Migrations;
using TestTaskToItKontact.Data.Repositories;
using TestTaskToItKontact.Data.Settings;
using TestTaskToItKontact.Domain.DataContracts;

namespace TestTaskToItKontact.Data.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataAccessOptions>(options =>
        {
            configuration.GetSection(nameof(DataAccessOptions)).Bind(options);
        });

        services.AddScoped<IFilesRepository, FileRepository>();

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddSqlServer()
                .WithGlobalConnectionString(s =>
                {
                    var cfg = s.GetRequiredService<IOptions<DataAccessOptions>>();
                    return cfg.Value.ConnectionString;
                })
                .ScanIn(typeof(DataExtensions).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }
}
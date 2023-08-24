using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Migrations
{
    static class Program
    {
        private const string AuroraDatabaseName = "AuroraDatabase";
        private static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IConfigurationSection connectionStrings = configuration.GetSection($"ConnectionStrings:{AuroraDatabaseName}");
            string connectionString = connectionStrings.Value ?? "";
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("The connection string is not defined. Please configure one in appsettings.");
            }
            IServiceProvider serviceProvider = CreateFluentMigratorService(connectionString);
            using IServiceScope scope = serviceProvider.CreateScope();
            if (args.Any(x => x.StartsWith("version:")))
            {
                string argument = args.FirstOrDefault(x => x.StartsWith("version:")) ?? "";
                if (!string.IsNullOrEmpty(argument))
                {
                    argument = argument.Replace("version:", "");
                    bool parsed = long.TryParse(argument, out long version);
                    if (parsed)
                    {
                        RollbackDatabase(scope.ServiceProvider, version);
                    }
                }
            }
            else
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            Console.WriteLine("Migration completed! ☕");
        }

        private static IServiceProvider CreateFluentMigratorService(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder =>
                    runnerBuilder
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(_000000000000_FirstRun).Assembly).For.Migrations())
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        private static void RollbackDatabase(IServiceProvider serviceProvider, long version)
        {
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateDown(version);
        }
    }
}
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using System;

namespace SNM.BrandManagement.Presentation.Configurations
{
    public static class SerilogConfiguration
    {
        public static void AddSerilogApi()
        {
            // Get the environment which the Facebook.Application is running on
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Get the configuration 
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                    .Build();

            // Create Logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails() // Adds details exception
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ElasticSearchConfiguration.ConfigureELS(configuration, env))
                .CreateLogger();
        }
    }
}
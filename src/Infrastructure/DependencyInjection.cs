using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;
using TaskManagement.Infrastructure.AWS;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Persistence.Interceptors;
using TaskManagement.Infrastructure.Persistence.Repositories;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            bool enableSensitiveDataLogging = bool.TryParse(
                configuration["DatabaseSettings:EnableSensitiveDataLogging"],
                out bool parsedValue) && parsedValue;

            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });

            options.EnableSensitiveDataLogging(enableSensitiveDataLogging);
            options.AddInterceptors(
                provider.GetRequiredService<AuditableEntityInterceptor>(),
                provider.GetRequiredService<SoftDeleteInterceptor>(),
                provider.GetRequiredService<DispatchDomainEventsInterceptor>());
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileStorageService, FileStorageService>();

        AWSOptions awsOptions = configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.AddAWSService<IAmazonSecretsManager>();
        services.AddTransient<ISecretsManagerService, SecretsManagerService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ApplicationDbContextInitializer>();

        return services;
    }
}
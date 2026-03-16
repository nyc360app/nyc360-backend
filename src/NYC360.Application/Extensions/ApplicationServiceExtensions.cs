using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using FluentValidation;

namespace NYC360.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // AutoMapper Setup
        // This will automatically scan the assembly for any classes that inherit from AutoMapper.Profile
        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = configuration.GetSection("ServiceKeys:AutoMapper").Value;
            cfg.AddMaps(assembly);
        });
        
        // FluentValidation Setup
        // This scans the assembly for all validator classes (those inheriting from AbstractValidator<T>)
        services.AddValidatorsFromAssembly(assembly);

        // MediatR Setup
        // This scans the assembly for all handlers (IRequestHandler<TRequest, TResponse>)
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = configuration.GetSection("ServiceKeys:MediatR").Value;
            cfg.RegisterServicesFromAssembly(assembly);
        });
            
        return services;
    }
}
using Application.Interfaces;
using Infrastructure.Photos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SECTION_NAME));
        services.AddScoped<IPhotoAccessor, PhotoAccessor>();

        return services;
    }
}

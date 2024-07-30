using Microsoft.Extensions.DependencyInjection;

namespace Template.Api;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddServices(services);
        AddMappingProfiles(services);

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        //services.AddScoped<IYourEntityService, YourEntityService>();
    }

    private static void AddMappingProfiles(IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(YourMapperProfile));
    }
}

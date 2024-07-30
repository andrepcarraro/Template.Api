using Microsoft.Extensions.DependencyInjection;

namespace Template.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            //services.AddScoped<IYourEntityProcessingService, YourEntityProcessingService>();

            return services;
        }
    }
}

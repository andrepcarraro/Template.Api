using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure;

namespace Template.Api;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("YourDB_Connection"), b => b.MigrationsAssembly("Template.Infrastructure")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<IYourEntityRepository, YourEntityRepository>();

        return services;
    }
}

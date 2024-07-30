using Template.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Template.Infrastructure;
internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    //public DbSet<YourEntityData> YourEntityData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


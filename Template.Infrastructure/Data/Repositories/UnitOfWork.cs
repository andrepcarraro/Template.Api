using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Template.Infrastructure;

internal class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    //private IYourEntityRepository? _yourEntityDataRepository;

    public AppDbContext Context => _context;
    //public IYourEntityRepository YourEntityRepository => _yourEntityDataRepository ??= new YourEntityRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

namespace Template.Infrastructure;
public interface IUnitOfWork: IDisposable
{
    //IYourEntityRepository YourEntityRepository { get; }
    Task<int> SaveChangesAsync();
}

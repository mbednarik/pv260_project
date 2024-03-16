namespace DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}
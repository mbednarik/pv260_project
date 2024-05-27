using FundParser.DAL.Models;

namespace FundParser.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByID(int id, CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetQueryable();

        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default);

        Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default);

        Task Delete(int id, CancellationToken cancellationToken = default);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
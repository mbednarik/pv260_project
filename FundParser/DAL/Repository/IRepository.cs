using FundParser.DAL.Models;

namespace FundParser.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIDAsync(int id, CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetQueryable();

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Delete(int id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
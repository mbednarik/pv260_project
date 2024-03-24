using FundParser.DAL.Models;

namespace FundParser.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByID(int id, CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetQueryable();

        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default);

        TEntity Insert(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
using FundParser.DAL.Exceptions;
using FundParser.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace FundParser.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly FundParserDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(FundParserDbContext dbcontext)
        {
            _context = dbcontext;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByID(int id, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet.FindAsync([id], cancellationToken: cancellationToken) ??
                throw new EntityNotFoundException($"Cannot find database entity {typeof(TEntity)} with id {id}");
            return result;
        }

        public virtual async Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return (await _dbSet.AddAsync(entity, cancellationToken)).Entity;
        }

        public virtual async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            TEntity entityToDelete = await _dbSet.FindAsync([id], cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException("Entity with given Id does not exist.");

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            ArgumentNullException.ThrowIfNull(entityToDelete);

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            ArgumentNullException.ThrowIfNull(entityToUpdate);

            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async virtual Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }
    }
}

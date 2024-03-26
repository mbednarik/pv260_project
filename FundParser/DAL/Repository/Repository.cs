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

        public async virtual Task<TEntity> GetByID(int id, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new Exception("Entity with given Id does not exist.");
            }

            return entity;
        }

        public virtual async Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new Exception("Argument entity is null");
            }

            return (await _dbSet.AddAsync(entity, cancellationToken)).Entity;
        }

        public virtual void Delete(int id)
        {
            TEntity? entityToDelete = _dbSet.Find(id);
            if (entityToDelete == null)
            {
                throw new Exception("Entity with given Id does not exist.");
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null)
            {
                throw new Exception("Argument entityToUpdate is null.");
            }

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

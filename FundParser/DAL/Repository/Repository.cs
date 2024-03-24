using DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
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

        public async virtual Task<TEntity> GetByID(int id)
        {
            TEntity? entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new Exception("Entity with given Id does not exist.");
            }

            return entity;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Argument entity is null");
            }

            var result = _dbSet.Add(entity);
            return result.Entity;
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

        public async virtual Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}

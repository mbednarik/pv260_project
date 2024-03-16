﻿using DAL.Models;

namespace DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByID(int id);
        IQueryable<TEntity> GetQueryable();
        Task<IEnumerable<TEntity>> GetAll();
        void Insert(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}

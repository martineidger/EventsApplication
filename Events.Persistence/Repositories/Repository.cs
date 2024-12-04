using Events.Authentications.AuthModels;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.HelperModels.Pagination;
using Events.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Events.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public virtual bool Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public IEnumerable<T> GetAll(ItemPageParameters parameters)
        {
            var set = PagedList<T>.Paginate(_context.Set<T>().ToList(),
                parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception($"No elements in the database");
            return set;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

       /* public IEnumerable<T> Take(int count)
        {
            return _context.Set<T>().Take(count);
        }

        public IEnumerable<T> Skip(int count)
        {
            return _context.Set<T>().Skip(count);
        }*/

        /*public IEnumerable<T> GetWithPagination(int page, int size)
        {
            var skipCount = (page - 1) * size;
            return _context.Set<T>().Skip(skipCount).Take(size);
        }


        public IEnumerable<T> Paginate(IEnumerable<T> source, int page, int size)
        {
            var skipCount = (page - 1) * size;
            return source.Skip(skipCount).Take(size);
        }*/

        /*public async Task AddAsync(T entity)
{
   await _context.Set<T>().AddAsync(entity);
   await _context.SaveChangesAsync();
}

public async Task DeleteAsync(int id)
{
   var entity = await _context.Set<T>().FindAsync(id);
   if (entity != null)
   {
       _context.Set<T>().Remove(entity);
       await _context.SaveChangesAsync();
   }
}

public async Task UpdateAsync(T entity)
{
   _context.Set<T>().Update(entity);
   await _context.SaveChangesAsync();
}

Task<T> IRepository<T>.GetByIdAsync(int id)
{
   return _context.Set<T>().FindAsync(id);
}



Task IRepository<T>.GetAllAsync()
{
   throw new NotImplementedException();
}*/

    }
}

using Events.Authentications.AuthModels;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.HelperModels.Pagination;
using Events.Persistence.DataBase;
using IdentityModel;
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

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            /*var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;*/
            //var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            /*var pagedList = PagedList<T>.Paginate(set, parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception("No elements in the database");*/

            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        

        /*public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }*/

    }
}

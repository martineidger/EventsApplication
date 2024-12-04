using Events.DTOs.HelperModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /*public Task GetByIdAsync(int id);
        public Task GetAllAsync();
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(int id);*/

        public T GetById(int id);
        
        public IEnumerable<T> GetAll(ItemPageParameters parameters);
        public bool Add(T entity);
        public void Update(T entity);
        public bool Delete(int id);
        /*public IEnumerable<T> Skip(int count);
        public IEnumerable<T> Take(int count);
        public IEnumerable<T> GetWithPagination(int page, int size);
        public IEnumerable<T> Paginate(IEnumerable<T> source, int page, int size);*/
    }
}

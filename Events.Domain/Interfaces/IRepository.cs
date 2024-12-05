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
       
        public T GetById(int id);
        
        public IEnumerable<T> GetAll(ItemPageParameters parameters);
        public bool Add(T entity);
        public void Update(T entity);
        public bool Delete(int id);
      
    }
}

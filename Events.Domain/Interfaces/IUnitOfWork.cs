using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo { get; }
        IEventRepository EventRepo { get; }
        void Commit();
        void BeginTransaction();
        void Rollback();
        Task? CommitAsync();
        Task? RollbackAsync();

    }
}

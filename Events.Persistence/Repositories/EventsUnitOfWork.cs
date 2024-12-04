using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.Repositories
{
    public class EventsUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepo {  get; }
        public IEventRepository EventRepo {  get; }
        private readonly AppDbContext _db;
        public EventsUnitOfWork(AppDbContext context, ITokenService tokenService, IMapper mapper, IAdminUserService adminUserService)
        {
            _db = context;
            UserRepo ??= new UserRepository(context, tokenService, mapper, adminUserService);
            EventRepo ??= new EventRepository(context);
        }
        public void BeginTransaction() => _db.BeginTransaction();
        public void Commit()
        {
            _db.SaveChanges();
            _db.CommitTransaction();
        }

        public async Task? CommitAsync()
        {
            await _db.SaveChangesAsync();
            await _db.CommitAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Rollback()
        {
            _db.RollbackTransaction();
        }

        public async Task? RollbackAsync()
        {
            await _db.RollbackTransactionAsync();
        }

        public bool Login(AuhorizationModel model)
        {
            throw new NotImplementedException();
        }

        //public bool Register(RegistrationModel user)
        //{
        //    if (UserRepo.Any(u => u.Email.Equals(user.Email)))
        //        return false;
        //    UserRepo.Add(user);
        //    Commit();
        //    return true;
        //}
    }
}

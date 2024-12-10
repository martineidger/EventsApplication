using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.Domain.Extensions;
using Events.Persistence.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Events.DTOs;
using Duende.IdentityServer.Models;
using System.Drawing;
using Events.DTOs.HelperModels.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Events.Persistence.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly AppDbContext _db;
        
        public event IEventRepository.EventChangedDelegate EventChanged;

        public EventRepository(AppDbContext context) : base(context)
        {
            _db = context;
            EventChanged += GenerateNotifications;
        }

        public void GenerateNotifications(int changedEventId, string message)
        {
            Console.WriteLine("------------Updating event---------");

            var changedEvent = _db.Events.Include(e => e.Users).FirstOrDefault(e => e.Id == changedEventId) ?? throw new Exception("No event with such ID");
            foreach(var user in changedEvent.Users)
            {
                user.Notifications.Add(
                    new Notification()
                    {
                        EventName = changedEvent.Name,
                        Message = message,
                        IsRead = false
                    });
            }
            _db.SaveChanges();
        }
       

        public async Task<Event> GetEventByNameAsync(string name)
        {
            return await _db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(DateTime dateTime)
        {
            /*var events = PagedList<Event>.Paginate(.ToList(),
                parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception($"No events on such date and time: {dateTime.ToLocalTime}");*/
            return _db.Events.AsNoTracking().Where(e => e.EventTime == dateTime);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(string place)
        {
            return await _db.Events.AsNoTracking().Where(e => e.Address == place).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(EventCategory eventCategory)
        {
            return await _db.Events.Where(e => e.Category == eventCategory).ToListAsync();
        }

        public EntityEntry<Event> Update(Event newEvent)
        {
            _db.Update(newEvent);
            return _db.Entry(newEvent);
        }
        public void OnEventChanged(int id, string message)
        {
            EventChanged?.Invoke(id, message);
        }

        public async Task<Event> GetByIdAsync(int id) 
        {
            return await _db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        

    }
}


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
       
        public override async Task<bool> AddAsync(Event newEvent)
        {
            if (_db.Events.Any(e => e.Name == newEvent.Name))
                return false;
            return await base.AddAsync(newEvent);
        }

        public async Task<Event> GetEventByNameAsync(string name)
        {
            return await _db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Name == name) ?? throw new Exception($"No events with such name: {name}");
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, DateTime dateTime)
        {
            var events = PagedList<Event>.Paginate(_db.Events.Where(e => e.EventTime == dateTime).ToList(),
                parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception($"No events on such date and time: {dateTime.ToLocalTime}");
            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, string place)
        {
            var events = PagedList<Event>.Paginate(_db.Events.Where(e => e.Address == place).ToList(),
                parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception($"No events on such place: {place}");
            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, EventCategory eventCategory)
        {
            var events = PagedList<Event>.Paginate(_db.Events.Where(e => e.Category == eventCategory).ToList(), 
                parameters.PageNumber, parameters.PageSize)
                ?? throw new Exception($"No events in this category: {eventCategory}");
            return events;
        }

        public async Task<bool> RegisterUserOnEventAsync(int curEventId, int userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId)?? throw new Exception($"No user with such ID: {userId}");
            var curEvent = _db.Events.FirstOrDefault(e => e.Id == curEventId) ?? throw new Exception($"No event with such ID :{curEventId}");

            if (curEvent.CanRegisterOnEvent())
            {
                curEvent.Users.Add(user);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }

        public async Task<bool> RemoveUserFromEventAsync(int curEventId, int userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId) ?? throw new Exception($"No user with such ID: {userId}");
            var curEvent = _db.Events.FirstOrDefault(e => e.Id == curEventId) ?? throw new Exception($"No event with such ID :{curEventId}");

            var result = curEvent.Users.Remove(user);
            _db.SaveChanges();
            return result;
        }

        public async Task<int> UpdateEventAsync(int curEventId, Event newEvent)
        {
            if (_db.Events.Find(curEventId) == null)
                throw new Exception($"No event with such ID :{curEventId}");
            newEvent.Id = curEventId;

            _db.Events.Update(newEvent);
            var entry = _db.Entry(newEvent);

            var modifiedProperties = entry.OriginalValues.Properties
                .Where(p => entry.Property(p.Name).IsModified)
                .ToDictionary(p => p.Name, p => entry.Property(p.Name).CurrentValue);

            foreach(var property in modifiedProperties)
            {
                EventChanged?.Invoke(curEventId, $"{property.Key} has been updated from to {property.Value}.");
            }

            _db.SaveChanges();
            return modifiedProperties.Count;
        }

        public async Task<Event> GetByIdAsync(int id) 
        {
            return await _db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        

    }
}


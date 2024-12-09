using Events.Domain.Entities;
using Events.DTOs;
using Events.DTOs.HelperModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        public delegate void EventChangedDelegate(int id, string message);
        public event EventChangedDelegate EventChanged;
        public Task<int> UpdateEventAsync(int curEventId, Event newEvent);
        public Task<Event> GetByIdAsync(int id);
        public Task<bool> RegisterUserOnEventAsync(int curEventId, int userId);
        public Task<bool> RemoveUserFromEventAsync(int curEventId, int userId);
        public Task<Event> GetEventByNameAsync(string name);
        public Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, DateTime dateTime);
        public Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, string place);
        public Task<IEnumerable<Event>> GetEventsAsync(ItemPageParameters parameters, EventCategory eventCategory);
        public void GenerateNotifications(int changedEventId, string message);

    }
}

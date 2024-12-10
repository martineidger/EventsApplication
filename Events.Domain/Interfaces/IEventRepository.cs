using Events.Domain.Entities;
using Events.DTOs;
using Events.DTOs.HelperModels.Pagination;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public EntityEntry<Event> Update(Event newEvent);
        public void OnEventChanged(int eventId, string message);
        public Task<Event> GetByIdAsync(int id);
        public Task<Event> GetEventByNameAsync(string name);
        public Task<IEnumerable<Event>> GetEventsAsync( DateTime dateTime);
        public Task<IEnumerable<Event>> GetEventsAsync(string place);
        public Task<IEnumerable<Event>> GetEventsAsync(EventCategory eventCategory);

    }
}

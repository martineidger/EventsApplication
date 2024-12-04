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
        public int UpdateEvent(int curEventId, Event newEvent);
        public bool RegisterUserOnEvent(int curEventId, int userId);
        public bool RemoveUserFromEvent(int curEventId, int userId);
        public Event GetEventByName(string name);
        public IEnumerable<Event> GetEvents(ItemPageParameters parameters, DateTime dateTime);
        public IEnumerable<Event> GetEvents(ItemPageParameters parameters, string place);
        public IEnumerable<Event> GetEvents(ItemPageParameters parameters, EventCategory eventCategory);
        public void GenerateNotifications(int changedEventId, string message);

    }
}

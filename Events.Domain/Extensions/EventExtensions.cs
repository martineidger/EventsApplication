using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Extensions
{
    public static class EventExtensions
    {
        public static bool CanRegisterOnEvent(this Event curEvent)
        {
            return curEvent.FreePlacesCount() > 0 &&
                   curEvent.EventTime >= DateTime.Now;
        }
        public static IEnumerable<User> GetUsersFromEvent(this Event curEvent)
        {
            return curEvent.Users;
        }
    }
}

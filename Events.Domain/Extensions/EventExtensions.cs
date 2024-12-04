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

        //public static bool Update(this Event source, Event target)
        //{
        //    if (source == null || target == null) return false;
        //    var changedProps = 0;

        //    PropertyInfo[] properties = typeof(Event).GetProperties();

        //    foreach (var property in properties)
        //    {
        //        var targetValue = property.GetValue(target);
        //        var value = property.GetValue(source);
        //        if (value != null &&
        //            !(value is string str && string.IsNullOrEmpty(str) &&
        //            (property is not IList<User>)))
        //        && !Equals(targetValue, GetDefaultValue(property.PropertyType))
        //        {
        //            property.SetValue(target, value);
        //            changedProps++;
        //        }
        //    }
        //    return changedProps != 0;
        //}

        /*private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }*/
    }
}

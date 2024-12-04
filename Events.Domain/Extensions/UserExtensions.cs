using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Extensions
{
    public static class UserExtensions
    {
        public static string ShowNotifications(this User user)
        {
            if (user == null) throw new Exception("User cannot be null");

            var notifications = string.Empty;
            foreach (var notification in user.Notifications)
            {
                notifications += "Event " + notification.EventName + ": " + notification.Message + "\n";
            }
            return notifications;
        }
    }
}

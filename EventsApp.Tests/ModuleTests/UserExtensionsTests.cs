using Events.Domain.Entities;
using Events.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventsApp.Tests.ModuleTests
{
    public class UserExtensionsTests
    {
        [Fact]
        public void ShowNotifications_UserIsNull_ThrowsException()
        {
            // Arrange
            User user = null;

            // Act & Assert
            Assert.Throws<Exception>(() => user.ShowNotifications());
        }

        [Fact]
        public void ShowNotifications_UserHasNoNotifications_ReturnsEmptyString()
        {
            // Arrange
            var user = new User
            {
                Notifications = new List<Notification>()
            };

            // Act
            var result = user.ShowNotifications();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ShowNotifications_UserHasNotifications_ReturnsFormattedString()
        {
            // Arrange
            var user = new User
            {
                Notifications = new List<Notification>
                {
                    new Notification
                    {
                        EventName = "Conference",
                        Message = "You are invited to the conference."
                    },
                    new Notification
                    {
                        EventName = "Meeting",
                        Message = "Don't forget the meeting tomorrow."
                    }
                }
            };

            // Act
            var result = user.ShowNotifications();

            // Assert
            var expected = "Event Conference: You are invited to the conference.\n" +
                           "Event Meeting: Don't forget the meeting tomorrow.\n";
            Assert.Equal(expected, result);
        }
    }
}


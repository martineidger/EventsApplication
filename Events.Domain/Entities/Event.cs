using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using System.Reflection;

namespace Events.Domain.Entities
{
    public enum EventCategory
    {
        WithoutCategory,
        Excursion,
        Concert,
        Exhibition,
        Lecture
    }
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; } /*= new DateTime(2022,02,22);*/
        public string Address { get; set; }
        public EventCategory Category { get; set; } /*= EventCategory.WithoutCategory;*/
        public int MaxLimit { get; set; }
        public virtual List<User> Users { get; set; } = new();
        public string? ImagePath { get; set; }

        public int FreePlacesCount()
        {
            return MaxLimit - Users.Count;
        }
        public bool AddPictureToEvent()
        {
            return true;
        }
        
    }
}

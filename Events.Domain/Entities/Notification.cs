﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        //public virtual Event Event { get; set; }
        public string EventName { get; set; }   
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
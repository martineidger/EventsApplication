using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.DTOs.DTOs
{
    public class EventDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string Address { get; set; }
        public string CategoryStr { get; set; }
        public int FreePlaces { get; set; }
        public IFormFile? Image { get; set; }
    }
}

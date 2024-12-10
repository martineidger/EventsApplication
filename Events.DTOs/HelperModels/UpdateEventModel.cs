using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.DTOs.HelperModels
{
    public class UpdateEventModel
    {
        public UpdateEventModel()
        {
            Name = string.Empty;
            Description = string.Empty;
            EventTime = DateTime.MinValue;
            Address = string.Empty;
            Category = string.Empty;
            MaxLimit = 0;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; } 
        public string Address { get; set; }
        public string Category { get; set; } 
        public int MaxLimit { get; set; }
        public IFormFile? Image { get; set; }
    }
}

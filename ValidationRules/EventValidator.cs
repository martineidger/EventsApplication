using Events.Domain.Entities;
using Events.DTOs.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.ValidationRules
{
    public class EventValidator : AbstractValidator<EventDTO>
    {
        private string[] categories = Enum.GetNames(typeof(EventCategory));
        public EventValidator()
        {
            RuleFor(ev => ev.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(ev => ev.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(ev => ev.EventTime).NotEmpty().WithMessage("Time is required.");
            RuleFor(ev => ev.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(ev => ev.CategoryStr).NotEmpty().WithMessage("Time is required.");
            RuleFor(ev => ev.FreePlaces).
                NotEmpty().
                GreaterThan(0).
                WithMessage("Free places must be greater than 0.");

            RuleFor(ev => ev.CategoryStr).
                NotEmpty().
                Must(cat => categories.Contains(cat)).
                WithMessage("Invalid category name.");

            RuleFor(ev => ev.EventTime).
                NotEmpty().
                Must(time => time > DateTime.Now).
                WithMessage("Event time must be in the future.");
        }

    }
}

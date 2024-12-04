using Events.Authentications.AuthModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationRules
{
    public class RegistrationValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Username is required");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(user => user.Surname).NotEmpty().WithMessage("Surname is required");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required.").
                Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$").
                WithMessage("Password must be at least 8 characters long, " +
                         "contain at least one uppercase letter, " +
                         "one lowercase letter, one number, and one special character."); 
            RuleFor(user=> user.BirthDate).NotEmpty().WithMessage("Birth date is required").
                Must(date => date.ToDateTime(TimeOnly.MinValue) >= DateTime.Today.AddYears(-120)).WithMessage("You must be less than the 120 years old");
        }

    }
}

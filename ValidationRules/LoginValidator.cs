using Events.Authentications.AuthModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationRules
{
    public class LoginValidator : AbstractValidator<AuhorizationModel>
    {
        public LoginValidator()
        {
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required.").
                EmailAddress().WithMessage("Invalid email format.");
            RuleFor(user => user.Password).
                NotEmpty().WithMessage("Password is required.").
                Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$").
                WithMessage("Password must be at least 8 characters long, " +
                         "contain at least one uppercase letter, " +
                         "one lowercase letter, one number, and one special character."); 
        }
    }
}

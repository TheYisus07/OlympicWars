using ContractsOW;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OlympicWarsClientProject.Validations
{
    public class Validator : AbstractValidator<Player>
    {
        public Validator()
        {
            RuleFor(x => x.nickName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty nickname field")
                .Length(5,20).WithMessage("Nickname between 5 and 20 characters");
            
            RuleFor(x => x.password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty password field")
                .Length(5, 250).WithMessage("Password between 5 and 50 characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty email field")
                .EmailAddress().WithMessage("It is not an email");
        }
    }
}

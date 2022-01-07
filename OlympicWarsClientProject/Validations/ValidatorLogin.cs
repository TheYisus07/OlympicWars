using ContractsOW;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicWarsClientProject.Validations
{
    public class ValidatorLogin : AbstractValidator<PlayerContract>
    {
        public ValidatorLogin()
        {
            RuleFor(x => x.nickName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty nickname field");

            RuleFor(x => x.password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty email field");

        }
    }
}

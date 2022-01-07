using System;
using ContractsOW;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicWarsClientProject.Validations
{
    public class ValidatorDeck : AbstractValidator<DeckContract>
    {
        public ValidatorDeck()
        {
            RuleFor(x => x.deckName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Empty nickname field")
                .Length(5, 20).WithMessage("Nickname between 5 and 20 characters");
        }
    }
}

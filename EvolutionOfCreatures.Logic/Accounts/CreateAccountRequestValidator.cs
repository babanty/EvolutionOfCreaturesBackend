using FluentValidation;
using Infrastructure.Tools.Validations;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class CreateAccountRequestValidator : NotNullEntityValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(p => p.Player)
                .NotNull()
                .WithMessage("Player can't be null");
        }
    }
}

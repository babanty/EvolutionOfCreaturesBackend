using FluentValidation;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(p => p.Player)
                .NotNull()
                .WithMessage("Player can't be null");
        }
    }
}

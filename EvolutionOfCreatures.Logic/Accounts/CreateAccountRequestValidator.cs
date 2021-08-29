using FluentValidation;
using Infrastructure.Tools.Validations;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class CreateAccountRequestValidator : NotNullEntityValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .WithMessage($"Account name can't be null");
        }
    }
}

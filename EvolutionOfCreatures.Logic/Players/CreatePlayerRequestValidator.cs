using FluentValidation;
using FluentValidation.Results;

namespace EvolutionOfCreatures.Logic.Players
{
    public class CreatePlayerRequestValidator : AbstractValidator<CreatePlayerRequest>
    {
        public CreatePlayerRequestValidator()
        {
            RuleFor(p => p.PlayerName)
                .NotEmpty()
                .WithMessage("Name can't be empty")
                .MinimumLength(2)
                .WithMessage("Name was too short")
                .MaximumLength(25)
                .WithMessage("Name was too long");

            RuleFor(p => p.AccountId)
                .NotNull()
                .WithMessage("AccountId can't be null")
                .NotEmpty()
                .WithMessage("AccountId can't be empty");
        }
    }
}

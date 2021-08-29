using FluentValidation;
using FluentValidation.Results;

namespace Infrastructure.Tools.Validations
{
    public abstract class NotNullEntityValidator<T> : AbstractValidator<T>
    {
        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(T), $"{nameof(T)} can't be null"));
                return false;
            }
            return base.PreValidate(context, result);
        }
    }
}

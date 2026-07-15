using FluentValidation;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Shared.Validators;

public static class PasswordValidator
{
    internal static IRuleBuilderOptions<TRequest, string> Password<TRequest>(this IRuleBuilderInitial<TRequest, string> ruleBuilder)
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PASSWORD_NOT_EMPTY)
            .MinimumLength(6)
            .WithMessage(ResourceMessagesException.PASSWORD_MIN_LENGTH);
    }
}

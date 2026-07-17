using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.TITLE_REQUIRED)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.TITLE_MAX_LENGTH);

        RuleFor(recipe => recipe.CookTime)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.COOKING_TIME_INVALID);

        RuleForEach(recipe => recipe.DishTypes)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.DISH_TYPE_INVALID);

        RuleFor(recipe => recipe.Ingredients)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INGREDIENTS_REQUIRED);

        RuleForEach(recipe => recipe.Ingredients)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INGREDIENT_EMPTY)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.INGREDIENT_MAX_LENGTH);

        RuleFor(recipe => recipe.Instructions)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INSTRUCTIONS_REQUIRED);

        RuleFor(recipe => recipe.Instructions)
            .Must(instructions => instructions.Select(instruction => instruction.Order).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessagesException.INSTRUCTIONS_ORDER_DUPLICATED)
            .When(recipe => recipe.Instructions.Count > 1);
    }
}

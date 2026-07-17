using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        var step = 1;

        return new Faker<RequestRecipeJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.CookTime, f => f.PickRandom<CookTime>())
            .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(r => r.DishTypes, f => f.Make(2, () => f.PickRandom<DishType>()))
            .RuleFor(r => r.Instructions, f => f.Make(3, () => new RequestRecipeInstructionJson
            {
                Description = f.Lorem.Sentence(),
                Order = step++,
            }));
    }
}

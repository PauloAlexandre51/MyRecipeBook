using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        TypeAdapterConfig<RequestRecipeJson, Recipe>
            .NewConfig()
            .Map(
            dest => dest.Ingredients, 
            source => source.Ingredients.Select(ingretiens => new RecipeIngredient
            {
                Item = ingretiens
            }))
            .Map(
            dest => dest.DishTypes,
            source => source.DishTypes.Select(dishType => new RecipeDishType
            {
                Type = (Domain.Enums.DishType)dishType
            }));
    }
}

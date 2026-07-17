using FluentMigrator;
using System.Data;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create tables to save the recipes information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Recipes")
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookTime").AsString(50).NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_Recipes_Users", "Users", "Id").OnDelete(Rule.Cascade);

        CreateTable("RecipeIngredients")
            .WithColumn("Item").AsString(255).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeIngredients_Recipes", "Recipes", "Id").OnDelete(Rule.Cascade);

        CreateTable("RecipeInstructions")
            .WithColumn("Order").AsInt32().NotNullable()
            .WithColumn("Description").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeInstructions_Recipes", "Recipes", "Id").OnDelete(Rule.Cascade);

        CreateTable("RecipeDishTypes")
            .WithColumn("Type").AsString(50).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeDishTypes_Recipes", "Recipes", "Id").OnDelete(Rule.Cascade);
    }
}
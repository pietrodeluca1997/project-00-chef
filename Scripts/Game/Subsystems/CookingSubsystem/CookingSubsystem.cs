using Game.Subsystems.CookingSubsystem.DTO;
using Game.Subsystems.CookingSubsystem.Entities;
using Game.Subsystems.CookingSubsystem.Factories;
using Godot;
using Project00ChefGame.Scripts.DevelopmentKit.File;
using Project00ChefGame.Scripts.Game.Subsystems.CookingSubsystem.Resources;
using System.Collections.Generic;
using System.Text.Json;

namespace Game.Subsystems.CookingSubystem.Scripts;

public partial class CookingSubsystem : Node
{
    private const string IngredientsJsonPath = "res://Scripts/Game/Subsystems/CookingSubsystem/Resources/Ingredients.json";
    private const string RecipesJsonPath = "res://Scripts/Game/Subsystems/CookingSubsystem/Resources/Recipes.json";

    private readonly JsonSerializerOptions DefaultJsonSerializationOptions = new() { PropertyNameCaseInsensitive = true };

    public List<CookingIngredientResource> Ingredients { get; private set; }

    public List<Recipe> Recipes { get; private set; }

    public void Initialize()
    {
        try
        {
            string ingredientsJson = FileHandler.ReadFileText(IngredientsJsonPath);
            string recipesJson = FileHandler.ReadFileText(RecipesJsonPath);

            Ingredients = JsonSerializer.Deserialize<List<CookingIngredientResource>>(ingredientsJson, DefaultJsonSerializationOptions);
            Recipes = RecipeFactory.FromJsonDto(JsonSerializer.Deserialize<List<RecipeJsonDto>>(recipesJson, DefaultJsonSerializationOptions));
        }
        catch
        {
            throw;
        }
    }
}

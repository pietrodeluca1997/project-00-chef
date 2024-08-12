using Game.Subsystems.CookingSubsystem.DTO;
using Game.Subsystems.CookingSubsystem.Entities;
using Game.Subsystems.CookingSubsystem.Entities.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Subsystems.CookingSubsystem.Factories;

public static class RecipeFactory
{
    public static List<Recipe> FromJsonDto(IEnumerable<RecipeJsonDto> recipesFromJson)
    {
        List<Recipe> recipes = new();

        try
        {
            foreach (RecipeJsonDto recipeDto in recipesFromJson)
            {
                Recipe recipe = new()
                {
                    Name = recipeDto.Name,
                    Requirements = new List<RecipeRequirement>()
                };

                foreach (RecipeRequirementJsonDto recipeRequirementDto in recipeDto.Requirements)
                {
                    recipe.Requirements.Add(new RecipeRequirement()
                    {
                        CookingIngredient = new CookingIngredient()
                        {
                            IngredientName = recipeRequirementDto.IngredientName
                        },
                        StatesRequired = recipeRequirementDto.RequiredStates.Select(state => (EIngredientState)Enum.Parse(typeof(EIngredientState), state)).ToList()
                    });
                }

                recipes.Add(recipe);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex.Message);
        }

        return recipes;
    }
}

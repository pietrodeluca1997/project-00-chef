using Game.Subsystems.CookingSubsystem.Entities.Enums;
using System.Collections.Generic;

namespace Game.Subsystems.CookingSubsystem.Entities;

public class RecipeRequirement
{
    public CookingIngredient CookingIngredient { get; set; }

    public List<EIngredientState> StatesRequired { get; set; }
}
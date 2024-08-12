using System.Collections.Generic;

namespace Game.Subsystems.CookingSubsystem.Entities;

public class Recipe
{
    public string Description { get; set; }

    public string Name { get; set; }

    public List<RecipeRequirement> Requirements { get; set; }
}

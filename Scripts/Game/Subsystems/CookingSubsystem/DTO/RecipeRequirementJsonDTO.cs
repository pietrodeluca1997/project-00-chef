using System.Collections.Generic;

namespace Game.Subsystems.CookingSubsystem.DTO;

public readonly record struct RecipeRequirementJsonDto(string IngredientName, IReadOnlyList<string> RequiredStates);
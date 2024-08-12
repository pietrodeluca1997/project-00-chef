using System.Collections.Generic;

namespace Game.Subsystems.CookingSubsystem.DTO;

public readonly record struct RecipeJsonDto(string Name, IReadOnlyList<RecipeRequirementJsonDto> Requirements);
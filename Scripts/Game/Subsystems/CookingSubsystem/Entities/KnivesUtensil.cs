using DevelopmentKit.EntityComponents.Interaction;
using Game.Subsystems.CookingSubsystem.Entities.Enums;

namespace Game.Subsystems.CookingSubsystem.Entities;

public partial class KnivesUtensil : UtensilBase
{
	protected override void ProcessEnded()
	{
		base.ProcessEnded();

		CurrentIngredientProcessing.CurrentStates.Remove(EIngredientState.Natural);
		CurrentIngredientProcessing.CurrentStates.Add(EIngredientState.Sliced);
		CurrentIngredientProcessing.Slice();
	}

	public override void _Ready()
	{
		base._Ready();
		ProcessWaitTime = 1.0f;
	}

	public override void Interact(IInteractantActor interactant)
	{
		if (interactant.InteractionComponent.CarryingObject is not CookingIngredient cookingIngredient)
			return;

		if (!cookingIngredient.CurrentStates.Contains(EIngredientState.Sliced))
		{
			StartProcessing(cookingIngredient);
		}
	}

	public override void StopInteraction(IInteractantActor interactant)
	{
		if (interactant.InteractionComponent.CarryingObject is not CookingIngredient)
			return;

		StopProcessing();
	}
}

using DevelopmentKit.EntityComponents.Interaction;
using Game.Subsystems.CookingSubsystem.Entities.Enums;

namespace Game.Subsystems.CookingSubsystem.Entities;

public partial class SinkUtensil : UtensilBase
{
    protected override void ProcessEnded()
    {
        base.ProcessEnded();

        CurrentIngredientProcessing.CurrentStates.Remove(EIngredientState.Natural);
        CurrentIngredientProcessing.CurrentStates.Add(EIngredientState.Washed);
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

        if (cookingIngredient.CurrentStates.Contains(EIngredientState.Natural))
        {
            StartProcessing(cookingIngredient);
            base.Interact(interactant);
        }
    }

    public override void StopInteraction(IInteractantActor interactant)
    {
        if (interactant.InteractionComponent.CarryingObject is not CookingIngredient)
            return;

        StopProcessing();
        base.StopInteraction(interactant);
    }
}

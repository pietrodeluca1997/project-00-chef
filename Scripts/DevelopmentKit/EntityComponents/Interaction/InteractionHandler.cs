using Godot;

namespace DevelopmentKit.EntityComponents.Interaction;

public class InteractionHandler
{
    public static void HandleBodyEntered(Node3D enteredBody, IInteractableObject interactableObject)
    {
        if (enteredBody is IInteractantActor interactant)
        {            
            interactant.InteractionComponent.AddObject(interactableObject);
        }
    }

    public static void HandleBodyExited(Node3D exitedBody, IInteractableObject interactableObject)
    {
        if (exitedBody is IInteractantActor interactant)
        {            
            interactant.InteractionComponent.RemoveObject(interactableObject);
        }
    }
}

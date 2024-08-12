using Godot;

namespace DevelopmentKit.EntityComponents.Interaction;

public interface IInteractableObject
{
    bool IsAvailableForInteraction { get; }

    MeshInstance3D MeshInstance { get; }

    void Interact(IInteractantActor interactant);

    void OnBodyEntered(Node3D body) => InteractionHandler.HandleBodyEntered(body, this);

    void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

    void StopInteraction(IInteractantActor interactant);
}
using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Signals;
using Godot;

namespace Project00ChefGame.Scripts.Game.Subsystems.SupplySubsystem;

public partial class SupplyTruck : Node3D, IInteractableObject
{
    public bool IsAvailableForInteraction { get; set; }
    public PathFollow3D TruckPath { get; set; }
    public bool Coming { get; set; }
    public bool Arrived { get; set; }

    [Export]
    public MeshInstance3D MeshInstance { get; set; }

    public delegate void PlayerInteractedEventSignature();
    public event PlayerInteractedEventSignature PlayerInteractionEvent;

    public override void _Ready()
    {
        IsAvailableForInteraction = false;

        TruckPath = GetParent<PathFollow3D>();

        SignalHelper.PrepareInteractableObject(this, "./Area3D");
    }

    public void Interact(IInteractantActor interactant)
    {
        if (IsAvailableForInteraction)
        {
            PlayerInteractionEvent?.Invoke();
        }
    }

    public void GoAway()
    {
        Arrived = false;
        Coming = true;
        TruckPath.ProgressRatio = 0.0f;
    }

    public void StopInteraction(IInteractantActor interactant)
    {

    }

    public void OnBodyEntered(Node3D body)
    {
        if (IsAvailableForInteraction)
            InteractionHandler.HandleBodyEntered(body, this);
    }

    public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

    public void Arrive()
    {
        IsAvailableForInteraction = true;
        Arrived = true;
        Coming = false;
    }
}

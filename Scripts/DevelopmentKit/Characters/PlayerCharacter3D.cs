using DevelopmentKit.EntityComponents.Interaction;
using Godot;
using GodotKit.EntityComponents.Interaction;

namespace DevelopmentKit.Characters;

public abstract partial class PlayerCharacter3D : CharacterBase3D, IInteractantActor
{
    [Export] public Node3D SpringArmOffset { get; protected set; }

    [Export] public SpringArm3D SpringArm { get; protected set; }

    public InteractionComponent InteractionComponent { get; protected set; }

    public override void _Ready()
    {
        InteractionComponent = GetNode<InteractionComponent>(nameof(InteractionComponent));
    }
}

using DevelopmentKit.StateMachines;
using Godot;
using GodotKit.EntityComponents.Locomotion;

namespace DevelopmentKit.Characters;

public abstract partial class CharacterBase3D : CharacterBody3D
{
    [ExportCategory("Base Object Associations")]
    [Export] public LocomotionComponent LocomotionComponent { get; protected set; }

    [Export] public Node3D CharacterBody { get; protected set; }

    [Export] public AnimationTree AnimationTree { get; protected set; }

    [Export] public AnimationPlayer AnimationPlayer { get; protected set; }

    public StateMachine StateMachine { get; protected set; }

    public override void _Ready()
    {
        StateMachine = new StateMachine();
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.Update((float)delta);
    }
}

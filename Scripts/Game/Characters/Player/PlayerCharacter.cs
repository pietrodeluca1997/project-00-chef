using DevelopmentKit.Characters;
using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.StateMachines;
using Game.Characters.Player.States;
using Godot;
using Project00ChefGame.Scripts.Game.PlayerStates;

namespace Game.Characters.Player;

[ScenePath("res://Scenes/Characters/Player.tscn")]
public partial class PlayerCharacter : PlayerCharacter3D
{
    public IState IdleCarryingState { get; protected set; }

    public IState IdleState { get; protected set; }

    public IState RunningState { get; protected set; }

    public IState WalkingCarryingState { get; protected set; }

    public IState WalkingState { get; protected set; }

    public IState BusyState { get; protected set; }

    [Export]
    public PlayerState PlayerState { get; protected set; }

    public override void _Ready()
    {
        base._Ready();

        IdleState = new IdlePlayerState(this, "Idle", "IdleWalk", "IsIdle");
        BusyState = new BusyPlayerState(this, "Idle", "IdleWalk", "IsIdle");
        WalkingState = new WalkingPlayerState(this, "Walk", "IdleWalk", "IsWalking");
        RunningState = new RunningPlayerState(this, "Running", "Running", "IsRunning");
        IdleCarryingState = new IdleCarryingPlayerState(this, "IdleCarry", "IdleWalkCarrying", "IsCarrying");
        WalkingCarryingState = new WalkCarryingPlayerState(this, "WalkCarry", "IdleWalkCarrying", "IsCarrying");

        StateMachine = new StateMachine();
        StateMachine.Initialize(IdleState);
    }
}

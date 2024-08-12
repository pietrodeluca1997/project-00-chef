using DevelopmentKit.Characters;
using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.Signals;
using DevelopmentKit.StateMachines;
using Game.Characters.Customers.States;
using Game.Managers;
using Game.Subsystems.RestaurantSubsystem.Entities;
using Godot;

namespace Game.Characters.Customers;

[ScenePath("res://Scenes/Characters/Customer.tscn")]
public partial class CustomerCharacter : CharacterBase3D, IInteractableObject
{

    public override void _Ready()
    {
        base._Ready();

        IsAvailableForInteraction = true;

        AnimationPlayer = GetNode<AnimationPlayer>("./Body/AnimationPlayer");
        NavigationAgent = GetNode<NavigationAgent3D>("./NavigationAgent3D");

        SeekingAvailableTableCustomerState = new SeekingAvailableTableCustomerState(this, "Idle");
        GoingToTableCustomerState = new GoingToTableCustomerState(this, "Walk");
        SittingOnChairCustomerState = new SittingOnChairCustomerState(this, "Sitting_Start");
        WaitingCustomerState = new WaitingCustomerState(this, "Sitting_Idle");
        CustomerEatingState = new CustomerEatingState(this, "Sitting_Eating");
        GettingUpFromChairCustomerState = new GettingUpFromChairCustomerState(this, "Sitting_End");
        GoingAwayState = new CustomerGoingAwayState(this, "Walk");

        StateMachine.Initialize(SeekingAvailableTableCustomerState);

        SignalHelper.PrepareInteractableObject(this, "./Area3D");
    }

    public void Interact(IInteractantActor interactant)
    {
        if (IsAvailableForInteraction)
        {
            if (StateMachine.CurrentState is WaitingCustomerState)
            {
                IsAvailableForInteraction = false;
                GameManager.Instance.RestaurantSubsystem.MakeOrder(this);
            }
        }
    }

    public void OnBodyEntered(Node3D body)
    {
        if (IsAvailableForInteraction)
            InteractionHandler.HandleBodyEntered(body, this);
    }

    public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

    public void StopInteraction(IInteractantActor interactant)
    {
    }

    public RestaurantTable CurrentTable { get; set; } = null!;

    public CustomerEatingState CustomerEatingState { get; private set; }

    public IState GettingUpFromChairCustomerState { get; private set; }

    public IState GoingAwayState { get; private set; }

    public GoingToTableCustomerState GoingToTableCustomerState { get; private set; }

    public bool IsAvailableForInteraction { get; set; }

    public NavigationAgent3D NavigationAgent { get; protected set; }

    public IState SeekingAvailableTableCustomerState { get; private set; }

    public IState SittingOnChairCustomerState { get; private set; }

    public IState WaitingCustomerState { get; private set; }
    [Export]
    public MeshInstance3D MeshInstance { get; set; }
}

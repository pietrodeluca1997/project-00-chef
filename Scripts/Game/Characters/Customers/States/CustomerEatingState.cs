using DevelopmentKit.StateMachines;
using Game.Subsystems.CookingSubsystem.Entities;
using Godot;

namespace Game.Characters.Customers.States;

public class CustomerEatingState : CharacterStateBase<CustomerCharacter>
{
    private Timer eatingTimer;


    public delegate void CustomerFinishedEatingEventSignature();
    public event CustomerFinishedEatingEventSignature CustomerFinishedEating;

    public CustomerEatingState(
        CustomerCharacter character,
        string animationParameterName,
        string animationBlendPositionKeyName = null,
        string animationStateConditionParameterName = null) : base(
        character,
        animationParameterName,
        animationBlendPositionKeyName,
        animationStateConditionParameterName)
    {
    }

    private void OnEatingFinished()
    {
        eatingTimer.QueueFree();

        if (Character.CurrentTable.PlaceableObject is KitchenPlate kitchenPlate)
        {
            kitchenPlate.Clear();

            Character.CurrentTable.FreeTable();
        }

        Character.StateMachine.ChangeState(Character.GettingUpFromChairCustomerState);
    }

    public override void Enter()
    {
        eatingTimer = new Timer();

        Character.AddChild(eatingTimer);

        eatingTimer.WaitTime = 15.0f;
        eatingTimer.OneShot = false;
        eatingTimer.Timeout += OnEatingFinished;

        eatingTimer.Start();

        Character.AnimationPlayer.Play(AnimationParameterName);
    }

    public override void Exit()
    {
        eatingTimer.Timeout -= OnEatingFinished;
        eatingTimer.QueueFree();
        eatingTimer = null;

        CustomerFinishedEating?.Invoke();
    }

    public override void Update(float deltaTime)
    {
    }
}
using DevelopmentKit.StateMachines;
using Godot;

namespace Game.Characters.Customers.States;

public class GoingToTableCustomerState : CharacterStateBase<CustomerCharacter>
{
    public GoingToTableCustomerState(
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

    public delegate void OnCustomerReachTableEventSignature(CustomerCharacter customer);

    public event OnCustomerReachTableEventSignature OnCustomerReachTable;

    private void OnTableChairReached()
    {
        OnCustomerReachTable?.Invoke(Character);
        Character.StateMachine.ChangeState(Character.SittingOnChairCustomerState);
    }

    public override void Enter()
    {
        Character.AnimationPlayer.Play(AnimationParameterName);

        Character.NavigationAgent.TargetPosition = Character.CurrentTable.Chair.GlobalPosition;

        Character.NavigationAgent.TargetReached += OnTableChairReached;
    }

    public override void Exit() { Character.NavigationAgent.TargetReached -= OnTableChairReached; }

    public override void Update(float deltaTime)
    {
        Vector3 currentPosition = Character.GlobalTransform.Origin;

        Vector3 nextPathPosition = Character.NavigationAgent.GetNextPathPosition();

        Vector3 movementDirection = (nextPathPosition - currentPosition).Normalized();

        Character.LookAt(movementDirection, Vector3.Up);

        Character.LocomotionComponent.ApplyGroundMovement(movementDirection, deltaTime, Character.LocomotionComponent.MovementSpeed);
    }
}
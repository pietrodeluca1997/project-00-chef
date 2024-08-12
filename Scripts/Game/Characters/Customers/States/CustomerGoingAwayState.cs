using DevelopmentKit.StateMachines;
using Game.Managers;
using Godot;

namespace Game.Characters.Customers.States;

public class CustomerGoingAwayState : CharacterStateBase<CustomerCharacter>
{
    public CustomerGoingAwayState(
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

    private void OnSpawnPointReached()
    {
        Character.QueueFree();
        GameManager.Instance.RestaurantSubsystem.CustomerCount--;
    }

    public override void Enter()
    {
        Character.AnimationPlayer.Play(AnimationParameterName);

        Character.NavigationAgent.TargetPosition = GameManager.Instance.RestaurantSubsystem
            .RetrieveRandomSpawnPosition();

        Character.NavigationAgent.TargetReached += OnSpawnPointReached;
    }

    public override void Exit() { Character.NavigationAgent.TargetReached -= OnSpawnPointReached; }

    public override void Update(float deltaTime)
    {
        Vector3 currentPosition = Character.GlobalTransform.Origin;

        Vector3 nextPathPosition = Character.NavigationAgent.GetNextPathPosition();

        Vector3 movementDirection = (nextPathPosition - currentPosition).Normalized();

        Character.LookAt(movementDirection, Vector3.Up);

        Character.LocomotionComponent.ApplyGroundMovement(movementDirection, deltaTime, Character.LocomotionComponent.MovementSpeed);
    }
}
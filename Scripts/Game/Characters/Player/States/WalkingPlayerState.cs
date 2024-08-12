using DevelopmentKit.StateMachines;
using Game.Managers;
using Godot;

namespace Game.Characters.Player.States;

public class WalkingPlayerState : CharacterStateBase<PlayerCharacter>
{
    public WalkingPlayerState(
        PlayerCharacter character,
        string animationParameterName,
        string animationBlendPositionKeyName = null,
        string animationStateConditionParameterName = null) : base(
        character,
        animationParameterName,
        animationBlendPositionKeyName,
        animationStateConditionParameterName)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
        Character.AnimationTree
            .Set(
                AnimationBlendPositionKeyName,
                new Vector3(Character.Velocity.X, 0.0f, Character.Velocity.Z).LengthSquared());

        float velocityLength = Character.Velocity.LengthSquared();

        if (velocityLength <= 0.001f)
        {
            Character.StateMachine.ChangeState(Character.IdleState);
        }

        if (InputManager.Instance.CurrentFrameInputValues.OnSprintStarted)
        {
            Character.StateMachine.ChangeState(Character.RunningState);
        }

        InputManager.Instance.PlayerController
            .CalculateMovementDirection(InputManager.Instance.CurrentFrameInputValues.MovementInput, deltaTime, Character.LocomotionComponent.MovementSpeed);
    }
}
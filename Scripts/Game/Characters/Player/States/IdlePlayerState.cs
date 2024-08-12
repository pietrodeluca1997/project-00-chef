using DevelopmentKit.StateMachines;
using Game.Managers;
using Godot;

namespace Game.Characters.Player.States;

public class IdlePlayerState : CharacterStateBase<PlayerCharacter>
{
    public IdlePlayerState(
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

    public override void Enter() { Character.AnimationTree.Set(AnimationBlendPositionKeyName, Vector3.Zero); }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
        InputManager.Instance.PlayerController
            .CalculateMovementDirection(InputManager.Instance.CurrentFrameInputValues.MovementInput, deltaTime, Character.LocomotionComponent.MovementSpeed);

        float velocityLength = Character.Velocity.LengthSquared();

        if (velocityLength > 0.001f && InputManager.Instance.CurrentFrameInputValues.OnSprintStarted)
        {
            Character.StateMachine.ChangeState(Character.RunningState);
        }

        if (velocityLength > 0.001f)
        {
            Character.StateMachine.ChangeState(Character.WalkingState);
        }
    }
}

using DevelopmentKit.StateMachines;
using Game.Managers;

namespace Game.Characters.Player.States;

public class RunningPlayerState : CharacterStateBase<PlayerCharacter>
{
    public RunningPlayerState(
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

    public override void Enter() { Character.AnimationTree.Set(AnimationStateConditionParameterName, true); }

    public override void Exit() { Character.AnimationTree.Set(AnimationStateConditionParameterName, false); }

    public override void Update(float deltaTime)
    {
        InputManager.Instance.PlayerController
            .CalculateMovementDirection(InputManager.Instance.CurrentFrameInputValues.MovementInput, deltaTime, Character.LocomotionComponent.SprintSpeed);

        float velocityLength = Character.Velocity.LengthSquared();

        if (velocityLength <= 0.001f || InputManager.Instance.CurrentFrameInputValues.OnSprintEnded)
        {
            Character.StateMachine.ChangeState(Character.IdleState);
        }
    }
}

using DevelopmentKit.StateMachines;
using Game.Managers;
using Godot;

namespace Game.Characters.Player.States;

public class IdleCarryingPlayerState : CharacterStateBase<PlayerCharacter>
{
    public IdleCarryingPlayerState(
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
        Character.AnimationTree.Set(AnimationStateConditionParameterName, true);
        Character.AnimationTree.Set(AnimationBlendPositionKeyName, Vector3.Zero);
    }

    public override void Exit() { Character.AnimationTree.Set(AnimationStateConditionParameterName, false); }

    public override void Update(float deltaTime)
    {
        InputManager.Instance.PlayerController
            .CalculateMovementDirection(InputManager.Instance.CurrentFrameInputValues.MovementInput, deltaTime, Character.LocomotionComponent.MovementSpeed);

        if (Character.Velocity.LengthSquared() > 0.001f)
        {
            Character.StateMachine.ChangeState(Character.WalkingCarryingState);
        }
    }
}
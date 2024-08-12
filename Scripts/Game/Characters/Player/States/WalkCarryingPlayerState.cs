using DevelopmentKit.StateMachines;
using Game.Managers;
using Godot;

namespace Game.Characters.Player.States;

public class WalkCarryingPlayerState : CharacterStateBase<PlayerCharacter>
{
    public WalkCarryingPlayerState(
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
        Character.AnimationTree
            .Set(AnimationBlendPositionKeyName, new Vector3(Character.Velocity.X, 0.0f, Character.Velocity.Z).Length());

        if (Character.Velocity.LengthSquared() < 0.001f)
        {
            Character.StateMachine.ChangeState(Character.IdleCarryingState);
        }

        InputManager.Instance.PlayerController
            .CalculateMovementDirection(InputManager.Instance.CurrentFrameInputValues.MovementInput, deltaTime, Character.LocomotionComponent.MovementSpeed);
    }
}
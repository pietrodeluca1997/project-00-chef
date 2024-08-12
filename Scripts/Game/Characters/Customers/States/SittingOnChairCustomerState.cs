using DevelopmentKit.StateMachines;
using Godot;

namespace Game.Characters.Customers.States;

public class SittingOnChairCustomerState : CharacterStateBase<CustomerCharacter>
{
    public SittingOnChairCustomerState(
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

    private void OnSittingStartAnimationFinished(StringName animationName)
    {
        if (animationName == AnimationParameterName)
        {
            Character.StateMachine.ChangeState(Character.WaitingCustomerState);
        }
    }

    public override void Enter()
    {
        Character.AnimationPlayer.Play(AnimationParameterName);

        Character.AnimationPlayer.AnimationFinished += OnSittingStartAnimationFinished;

        Transform3D chairTransform = Character.CurrentTable.Chair.GlobalTransform;

        Vector3 sitPosition = chairTransform.Origin + chairTransform.Basis.X * 0.5f;

        Character.GlobalPosition = sitPosition;

        Character.CharacterBody.LookAt(Character.CurrentTable.Chair.GlobalTransform.Origin, Vector3.Up);
    }

    public override void Exit() { Character.AnimationPlayer.AnimationFinished -= OnSittingStartAnimationFinished; }


    public override void Update(float deltaTime)
    {
    }
}
using DevelopmentKit.StateMachines;
using Godot;

namespace Game.Characters.Customers.States;

public class GettingUpFromChairCustomerState : CharacterStateBase<CustomerCharacter>
{
    public GettingUpFromChairCustomerState(
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

    private void OnGettingUpAnimationFinished(StringName animationName)
    {
        if (animationName == AnimationParameterName)
        {
            Character.GlobalPosition = Character.CurrentTable.GetUpLocation.GlobalPosition;

            Character.StateMachine.ChangeState(Character.GoingAwayState);
        }
    }

    public override void Enter()
    {
        Character.AnimationPlayer.Play(AnimationParameterName);

        Character.AnimationPlayer.AnimationFinished += OnGettingUpAnimationFinished;
    }

    public override void Exit() { Character.AnimationPlayer.AnimationFinished -= OnGettingUpAnimationFinished; }


    public override void Update(float deltaTime)
    {
    }
}
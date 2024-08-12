using DevelopmentKit.StateMachines;

namespace Game.Characters.Customers.States;

public class WaitingCustomerState : CharacterStateBase<CustomerCharacter>
{
    public WaitingCustomerState(
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

    public override void Enter() { Character.AnimationPlayer.Play(AnimationParameterName); }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
    }
}
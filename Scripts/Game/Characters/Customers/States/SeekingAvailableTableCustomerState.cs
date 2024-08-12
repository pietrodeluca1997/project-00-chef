using DevelopmentKit.StateMachines;
using Game.Managers;
using Game.Subsystems.RestaurantSubsystem.Entities;

namespace Game.Characters.Customers.States;

public class SeekingAvailableTableCustomerState : CharacterStateBase<CustomerCharacter>
{
    public SeekingAvailableTableCustomerState(
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
        if (!GameManager.Instance.RestaurantSubsystem.RetrieveAvailableTable(out RestaurantTable availableTable))
            return;

        Character.CurrentTable = availableTable;
        Character.StateMachine.ChangeState(Character.GoingToTableCustomerState);
    }
}
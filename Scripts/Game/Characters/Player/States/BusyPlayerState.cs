using DevelopmentKit.StateMachines;
using Godot;

namespace Game.Characters.Player.States;

public class BusyPlayerState : CharacterStateBase<PlayerCharacter>
{
    public BusyPlayerState(
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

    }
}
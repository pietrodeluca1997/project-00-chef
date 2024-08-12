using DevelopmentKit.Characters;

namespace DevelopmentKit.StateMachines;

public abstract class CharacterStateBase<TCharacterClass> : IState where TCharacterClass : CharacterBase3D
{
    protected CharacterStateBase(
        TCharacterClass character,
        string animationParameterName,
        string animationBlendPositionKeyName = null,
        string animationStateConditionParameterName = null)
    {
        AnimationParameterName = animationParameterName;

        if (!string.IsNullOrWhiteSpace(animationBlendPositionKeyName))
        {
            AnimationBlendPositionKeyName = $"parameters/{animationBlendPositionKeyName}/blend_position";
        }

        if (!string.IsNullOrWhiteSpace(animationStateConditionParameterName))
        {
            AnimationStateConditionParameterName = $"parameters/conditions/{animationStateConditionParameterName}";
        }

        Character = character;
    }

    protected string AnimationBlendPositionKeyName { get; } = null!;

    protected string AnimationParameterName { get; }

    protected string AnimationStateConditionParameterName { get; } = null!;

    protected TCharacterClass Character { get; }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update(float deltaTime);
}

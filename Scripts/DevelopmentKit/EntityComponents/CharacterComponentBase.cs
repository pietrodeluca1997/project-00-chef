using DevelopmentKit.Characters;
using Godot;

namespace GodotKit.EntityComponents;

public abstract partial class CharacterComponentBase<TCharacterClass> : Node where TCharacterClass : CharacterBase3D
{
    public TCharacterClass OwnerCharacter { get; protected set; }

    public override void _Ready() { OwnerCharacter = GetParent<TCharacterClass>(); }
}

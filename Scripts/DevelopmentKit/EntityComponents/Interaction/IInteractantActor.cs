using GodotKit.EntityComponents.Interaction;

namespace DevelopmentKit.EntityComponents.Interaction;

public interface IInteractantActor
{
    InteractionComponent InteractionComponent { get; }
}
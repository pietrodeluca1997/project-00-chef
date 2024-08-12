namespace DevelopmentKit.EntityComponents.Interaction;

public interface ICarriableObject : IInteractableObject
{
    void Carry();
    void Drop();
}
using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Nodes;
using Game.Characters.Player;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace GodotKit.EntityComponents.Interaction;

public partial class InteractionComponent : CharacterComponentBase<PlayerCharacter>
{
    private List<IInteractableObject> InteractableObjectsInArea { get; set; }
    private IInteractableObject ClosestInteractableObject { get; set; }
    public ShaderMaterial InteractableOutlineShader { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        InteractableOutlineShader = ResourceLoader.Load<ShaderMaterial>("res://Materials/HighlightMaterial.tres");

        InteractableObjectsInArea = new();
        CarriablePlaceLocation = GetParent()
            .GetNode<Marker3D>("./Character/CharacterArmature/Skeleton3D/CarriablePlaceLocation");
    }

    public void AddObject(IInteractableObject interactable) 
    {
        InteractableObjectsInArea.Add(interactable);
        OnInteractableObjectsChanged();
    }

    public void RemoveObject(IInteractableObject interactable)
    {
        if (InteractingObject is not null && interactable == InteractingObject)
        {
            InteractingObject.StopInteraction(OwnerCharacter);
            InteractingObject = null;
        }

        InteractableObjectsInArea.Remove(interactable);
        OnInteractableObjectsChanged();
    }

    public void TryCarry(ICarriableObject carriableObject)
    {
        TryDropCarryingObject();

        Node3D carriableObjectNode = carriableObject as Node3D;

        carriableObject.Carry();

        NodeManipulationHandler.Reparent(carriableObjectNode, CarriablePlaceLocation);
        NodeManipulationHandler.RepositionGlobally(carriableObjectNode, CarriablePlaceLocation.GlobalTransform);

        InteractableObjectsInArea.Remove(carriableObject);

        CarryingObject = carriableObject;

        OwnerCharacter.StateMachine.ChangeState(OwnerCharacter.IdleCarryingState);
    }

    public ICarriableObject TryDropCarryingObject()
    {
        if (CarryingObject is null || IsOwnerBusy || CarryingObject is not Node3D carryingObjectnode)
            return null;

        carryingObjectnode.GetParent()?.RemoveChild(carryingObjectnode);
        GetTree().Root.AddChild(carryingObjectnode);

        NodeManipulationHandler.RepositionGlobally(carryingObjectnode, CarriablePlaceLocation.GlobalTransform);

        CarryingObject.Drop();

        ICarriableObject carriableCopy = CarryingObject;

        CarryingObject = null;

        OwnerCharacter.StateMachine.ChangeState(OwnerCharacter.IdleState);

        return carriableCopy;
    }

    public void TryInteract()
    {
        if (!InteractableObjectsInArea.Any())
            return;

        InteractingObject = InteractableObjectsInArea.First();
        InteractingObject.Interact(OwnerCharacter);
    }

    public void TryStopCurrentInteraction() { InteractingObject?.StopInteraction(OwnerCharacter); }

    public Marker3D CarriablePlaceLocation { get; protected set; }

    public ICarriableObject CarryingObject { get; private set; }

    public IInteractableObject InteractingObject { get; private set; }

    public bool IsOwnerBusy { get; set; }

    private void SortInteractableObjectsByDistance()
    {
        InteractableObjectsInArea = InteractableObjectsInArea.OrderBy(obj =>
        {
            if (obj is Node3D node)
            {
                Vector3 objectPosition = node.GlobalTransform.Origin;
                Vector3 characterPosition = OwnerCharacter.GlobalTransform.Origin;
                return characterPosition.DistanceTo(objectPosition);
            }
            else
            {
                return float.MaxValue;
            }
        }).ToList();
    }

    private void OnInteractableObjectsChanged()
    {
        SortInteractableObjectsByDistance();
        UpdateNearestObjectHighlight();
    }

    private void UpdateNearestObjectHighlight()
    {
        if (InteractableObjectsInArea.Any())
        {
            IInteractableObject nearestObject = InteractableObjectsInArea.First();

            nearestObject.MeshInstance.MaterialOverlay = InteractableOutlineShader;

            if(ClosestInteractableObject is not null)
            {
                ClosestInteractableObject.MeshInstance.MaterialOverlay = null;
            }

            ClosestInteractableObject = nearestObject;
        }
        else if(ClosestInteractableObject is not null)
        {
            ClosestInteractableObject.MeshInstance.MaterialOverlay = null;
            ClosestInteractableObject = null;
        }
    }
}

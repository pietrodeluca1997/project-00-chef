using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Nodes;
using DevelopmentKit.Signals;
using Godot;

namespace Game.Subsystems.CookingSubsystem.Entities;

public partial class Bench : Node3D, IInteractableObject
{
	private Area3D interactionArea;
	private Marker3D placeableLocation;

	public override void _Ready()
	{
		IsAvailableForInteraction = true;
		interactionArea = GetNode<Area3D>("./Area3D");
		placeableLocation = GetNode<Marker3D>("./PlaceableLocation");

		SignalHelper.PrepareInteractableObject(this, "./Area3D");
	}

	public virtual void Interact(IInteractantActor interactant)
	{
		if (!IsAvailableForInteraction) return;

		if (PlaceableObject is null)
		{
			ICarriableObject droppedObject = interactant.InteractionComponent.TryDropCarryingObject();

			if (droppedObject is not null)
			{
				Node3D droppedObjectNode = droppedObject as Node3D;
				NodeManipulationHandler.Reparent(droppedObjectNode, placeableLocation);
				NodeManipulationHandler.RepositionGlobally(droppedObjectNode, placeableLocation.GlobalPosition);

				droppedObject.Carry();

				PlaceableObject = droppedObject;
			}
		}
		else
		{
			if (PlaceableObject is KitchenPlate kitchenPlate && interactant.InteractionComponent.CarryingObject is CookingIngredient)
			{
				kitchenPlate.Interact(interactant);                
			}
			else
			{
				interactant.InteractionComponent.TryCarry(PlaceableObject);
				PlaceableObject = null;
			}
		}
	}

	public void OnBodyEntered(Node3D body)
	{
		if (IsAvailableForInteraction)
			InteractionHandler.HandleBodyEntered(body, this);
	}

	public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

	public virtual void StopInteraction(IInteractantActor interactant)
	{
	}

	public bool IsAvailableForInteraction { get; set; }

	public ICarriableObject PlaceableObject { get; set; } = null!;

	[Export]
	public MeshInstance3D MeshInstance { get; set; }
}

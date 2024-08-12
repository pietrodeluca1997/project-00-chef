using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Nodes;
using DevelopmentKit.Signals;
using Godot;
using Project00ChefGame.Scripts.DevelopmentKit.Physics;
using System.Collections.Generic;
using System.Linq;

namespace Game.Subsystems.CookingSubsystem.Entities;

public partial class KitchenPlate : RigidBody3D, ICarriableObject
{
	public Stack<CookingIngredient> CurrentIngredients { get; private set; }

	public Marker3D IngredientPlaceLocation { get; private set; }

	public bool IsAvailableForInteraction { get; set; }

	[Export]
	public MeshInstance3D MeshInstance { get; set; }

	public static bool operator !=(KitchenPlate plate, Recipe recipe)
	{
		return !(plate == recipe);
	}

	public static bool operator ==(KitchenPlate plate, Recipe recipe)
	{
		if (plate is null || recipe is null)
		{
			return false;
		}

		if (plate.CurrentIngredients.Count != recipe.Requirements.Count)
		{
			return false;
		}

		foreach (CookingIngredient ingredient in plate.CurrentIngredients)
		{
			if (!recipe.Requirements
				.Any(
					req => req.CookingIngredient.IngredientName == ingredient.IngredientName &&
						req.StatesRequired.All(ingredient.CurrentStates.Contains)))
			{
				return false;
			}
		}

		return true;
	}

	public override void _Ready()
	{
		IsAvailableForInteraction = true;
		IngredientPlaceLocation = GetNode<Marker3D>("./Plate/IngredientPlaceLocation");
		CurrentIngredients = new Stack<CookingIngredient>();

		SignalHelper.PrepareInteractableObject(this, "./Area3D");
	}


	public void Carry()
	{
		IsAvailableForInteraction = false;
		this.FreezePhysics();
	}

	public void Clear()
	{
		IngredientPlaceLocation.GetChildren()
			.ToList()
			.ForEach(
				child =>
				{
					child.GetParent().RemoveChild(child);
					child.QueueFree();
				});

		IsAvailableForInteraction = true;
		CurrentIngredients.Clear();
	}

	public void Drop()
	{
		IsAvailableForInteraction = true;
		this.UnfreezePhysics();
	}

	public void Interact(IInteractantActor interactant)
	{
		if (interactant.InteractionComponent.CarryingObject is null)
		{
			interactant.InteractionComponent.TryCarry(this);
			return;
		}

		if (interactant.InteractionComponent.CarryingObject is not CookingIngredient cookingIngredient)
			return;

		interactant.InteractionComponent.TryDropCarryingObject();

		Node3D cookingIngredientMesh = cookingIngredient.IngredientNode;

		NodeManipulationHandler.Reparent(cookingIngredientMesh, IngredientPlaceLocation);
		NodeManipulationHandler.RepositionGlobally(cookingIngredientMesh, IngredientPlaceLocation.GlobalPosition);

		CurrentIngredients.Push(cookingIngredient);
	}

	public void OnBodyEntered(Node3D body)
	{
		if (IsAvailableForInteraction)
			InteractionHandler.HandleBodyEntered(body, this);
	}

	public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

	public void StopInteraction(IInteractantActor interactant)
	{
	}
}

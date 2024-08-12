using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.Signals;
using Game.Subsystems.CookingSubsystem.Entities.Enums;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Subsystems.CookingSubsystem.Entities;

[ScenePath("res://Scenes/Actors/Ingredient.tscn")]
public partial class CookingIngredient : RigidBody3D, ICarriableObject
{
    public override void _Ready()
    {
        UID = Guid.NewGuid();
        IsAvailableForInteraction = true;
        IngredientMeshScene = ResourceLoader.Load<PackedScene>(IngredientMeshPath);
        IngredientNode = IngredientMeshScene.Instantiate<Node3D>();
        MeshInstance = IngredientNode.GetChild(0).GetChild<MeshInstance3D>(0);
        AddChild(IngredientNode);

        CurrentStates = new List<EIngredientState> { EIngredientState.Natural };

        SignalHelper.PrepareInteractableObject(this, "./");
    }

    public void AddState(EIngredientState state) { CurrentStates.Add(state); }

    public void Carry()
    {
        IsAvailableForInteraction = false;
        FreezeMode = FreezeModeEnum.Static;
        Freeze = true;
    }

    public void Drop()
    {
        IsAvailableForInteraction = true;
        FreezeMode = FreezeModeEnum.Kinematic;
        Freeze = false;
    }

    public void Interact(IInteractantActor interactant) { interactant.InteractionComponent.TryCarry(this); }

    public void OnBodyEntered(Node3D body)
    {
        if (IsAvailableForInteraction)
            InteractionHandler.HandleBodyEntered(body, this);
    }

    public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

    public void Slice()
    {
        PackedScene slicedIngredientScene = ResourceLoader.Load<PackedScene>(SlicedIngredientMeshPath);

        IngredientNode.GetParent().RemoveChild(IngredientNode);
        IngredientNode.Free();

        IngredientNode = slicedIngredientScene.Instantiate<Node3D>();
        MeshInstance = IngredientNode.GetChild(0).GetChild<MeshInstance3D>(0);

        AddChild(IngredientNode);
    }

    public void StopInteraction(IInteractantActor interactant)
    {
    }

    public List<EIngredientState> CurrentStates { get; set; }

    [Export]
    public string IngredientMeshPath { get; set; }

    public PackedScene IngredientMeshScene { get; set; }

    [Export]
    public string IngredientName { get; set; }

    public Node3D IngredientNode { get; set; }

    public bool IsAvailableForInteraction { get; private set; }

    [Export]
    public string SlicedIngredientMeshPath { get; set; }

    public Guid UID { get; private set; }

    public MeshInstance3D MeshInstance { get; set; }
}

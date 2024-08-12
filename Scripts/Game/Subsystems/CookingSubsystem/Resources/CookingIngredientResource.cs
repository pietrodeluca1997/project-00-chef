using Godot;
using System;

namespace Project00ChefGame.Scripts.Game.Subsystems.CookingSubsystem.Resources;

public sealed partial class CookingIngredientResource : Resource
{
    public Guid UID { get; private set; }

    public string Name { get; set; }

    public string IngredientMeshPath { get; set; }

    public string SlicedIngredientMeshPath { get; set; }

    public float PurchasePrice { get; set; }

    public CookingIngredientResource()
    {
        UID = Guid.NewGuid();
    }
}

using DevelopmentKit.EntityComponents.Interaction;
using Game.Managers;
using Game.Subsystems.CookingSubsystem.Entities;
using Godot;
using System.Linq;

namespace Game.Subsystems.RestaurantSubsystem.Entities;

public sealed partial class RestaurantTable : Bench
{
    public override void _Ready()
    {
        base._Ready();

        GetUpLocation = GetNode<Marker3D>("./GetUpLocation");

        Number = ++NumberCounter;

        IsAvailable = true;
        Chair = GetNode<Node3D>("./Chair");
    }

    public override void Interact(IInteractantActor interactant)
    {
        if (!IsAvailableForInteraction) return;

        base.Interact(interactant);

        if (PlaceableObject is KitchenPlate kitchenPlate)
        {
            RestaurantOrder tableOrder = GameManager.Instance.RestaurantSubsystem.RestaurantOrders
                .FirstOrDefault(order => order.Table.Number == Number);

            if (tableOrder != null)
            {
                if (kitchenPlate == tableOrder.Request)
                {
                    IsAvailableForInteraction = false;
                    GameManager.Instance.RestaurantSubsystem.OrderFullFiled(tableOrder);
                }
            }
        }
    }

    public void FreeTable()
    {
        IsAvailableForInteraction = true;
        IsAvailable = true;
    }

    public void Retrieve() { IsAvailable = false; }

    public Node3D Chair { get; private set; }

    public Marker3D GetUpLocation { get; private set; }

    public bool IsAvailable { get; set; }

    public int Number { get; private set; }

    public static int NumberCounter { get; private set; } = 0;
}

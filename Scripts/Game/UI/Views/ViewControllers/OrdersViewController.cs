using DevelopmentKit.Reflection.Attributes;
using Game.Subsystems.RestaurantSubsystem.Entities;
using Game.UI.ViewComponents;
using Godot;
using GodotKit.UI.ViewControllers;
using System.Collections.Generic;

namespace Game.UI.Views.ViewControllers;

public partial class OrdersViewController : ViewControllerBase
{
    private Dictionary<RestaurantOrder, OrderViewComponent> OrderComponentsMapByOrder { get; set; }

    private PackedScene OrderPackedScene { get; set; }

    private Control OrdersParentContainer { get; set; }

    public override void _Ready()
    {
        OrderPackedScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<OrderViewComponent>());
        OrdersParentContainer = GetNode<Control>("./Background/MarginContainer/HBoxContainer/VBoxContainer");
        OrderComponentsMapByOrder = new();
    }

    public void CreateOrderComponent(RestaurantOrder order)
    {
        OrderViewComponent orderComponent = OrderPackedScene.Instantiate<OrderViewComponent>();

        OrdersParentContainer.AddChild(orderComponent);

        orderComponent.OrderNameLabel.Text = order.Request.Name;
        orderComponent.TableNumberLabel.Text = order.Table.Number.ToString();

        OrderComponentsMapByOrder.Add(order, orderComponent);
    }

    public void RemoveOrderComponent(RestaurantOrder order)
    {
        OrderComponentsMapByOrder[order].QueueFree();
        OrderComponentsMapByOrder.Remove(order);
    }
}

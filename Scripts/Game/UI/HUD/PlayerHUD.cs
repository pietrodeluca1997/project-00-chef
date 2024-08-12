using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.UI.HUD;
using Game.Managers;
using Game.Subsystems.RestaurantSubsystem.Entities;
using Game.UI.Views.ViewControllers;
using Godot;

namespace Game.UI.HUD;

[ScenePath("res://UI/HUD/PlayerHUD.tscn")]
public partial class PlayerHUD : HUDBase
{
    public OrdersViewController OrdersViewController { get; private set; }

    public SettingsViewController SettingsViewController { get; private set; }

    public CoinsViewController CoinsViewController { get; private set; }

    public PackedScene SupplyTradeViewScene { get; private set; }

    public Control SupplyTradeViewParent { get; private set; }

    public SupplyTradeViewController SupplyTradeViewController { get; private set; }

    public bool IsTradingSupplies { get; private set; }

    public override void _Ready()
    {
        SettingsViewController = GetNode<SettingsViewController>("./SettingsView");
        SettingsViewController.Hide();

        OrdersViewController = GetNode<OrdersViewController>("./OrdersView");

        CoinsViewController = GetNode<CoinsViewController>("./CoinsView");

        SupplyTradeViewParent = GetNode<Control>("./SupplyTradeViewParent");
    }

    public override void Initialize()
    {
        IsTradingSupplies = false;

        GameManager.Instance.PlayerController.OnUiCancel += OnUiCancel;
        GameManager.Instance.RestaurantSubsystem.OnOrderRequest += OnOrderRequest;
        GameManager.Instance.RestaurantSubsystem.OnOrderFullFiled += OnOrderFullFiled;
        GameManager.Instance.PlayerCharacter.PlayerState.CurrencyChangedEvent += CoinsViewController.UpdateCurrency;
        GameManager.Instance.SupplySubsystem.Truck.PlayerInteractionEvent += OnPlayerInteractedWithSupplyTruck;

        SupplyTradeViewScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<SupplyTradeViewController>());
    }

    public void OnOrderFullFiled(RestaurantOrder order) { OrdersViewController.RemoveOrderComponent(order); }

    public void OnOrderRequest(RestaurantOrder order) { OrdersViewController.CreateOrderComponent(order); }

    public void OnUiCancel()
    {
        SettingsViewController.ToggleVisibility();
        Input.MouseMode = SettingsViewController.Visible ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
    }

    public void OnPlayerInteractedWithSupplyTruck()
    {
        IsTradingSupplies = !IsTradingSupplies;

        if (IsTradingSupplies)
        {
            GameManager.Instance.PlayerCharacter.StateMachine.ChangeState(GameManager.Instance.PlayerCharacter.BusyState);
            SupplyTradeViewController = SupplyTradeViewScene.Instantiate<SupplyTradeViewController>();

            SupplyTradeViewParent.AddChild(SupplyTradeViewController);

            SupplyTradeViewController.CreateView();

            Input.WarpMouse(Vector2.Zero);
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            SupplyTradeViewController.QueueFree();

            GameManager.Instance.PlayerCharacter.StateMachine.ChangeState(GameManager.Instance.PlayerCharacter.IdleState);

            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}

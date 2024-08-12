using DevelopmentKit.Reflection.Attributes;
using Game.Characters.Customers;
using Game.Managers;
using Game.Subsystems.RestaurantSubsystem.Entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Subsystems.RestaurantSubsystem;

public sealed partial class RestaurantSubsystem : Node
{
    private const int CustomerLimit = 5;
    private const int CustomerSpawnCooldownInSeconds = 15;

    public delegate void OrderFulfilledEventSignature(RestaurantOrder order);

    public delegate void OrderRequestedEventSignature(RestaurantOrder order);

    public event OrderFulfilledEventSignature OnOrderFullFiled;

    public event OrderRequestedEventSignature OnOrderRequest;

    private void InitializeCustomerSpawner()
    {
        CustomerSpawnArea = GetTree().Root
            .GetNode<Area3D>("./SceneRoot/CustomerSpawnArea")
            .GetNode<CollisionShape3D>("./CollisionShape3D");
        CustomerSpawnAreaShape = CustomerSpawnArea.Shape as BoxShape3D;

        CustomerSpawnTimer = new Timer();
        AddChild(CustomerSpawnTimer);
        CustomerSpawnTimer.WaitTime = CustomerSpawnCooldownInSeconds;
        CustomerSpawnTimer.Timeout += SpawnCustomer;
        CustomerSpawnTimer.Start();
    }

    private void NotifyCustomerReached(CustomerCharacter customerReached)
    { SoundManager.Instance.PlayCustomerReachedNotification(customerReached.GlobalPosition); }

    private void SpawnCustomer()
    {
        if (CustomerCount >= CustomerLimit)
            return;

        CustomerCharacter newCustomer = CustomerPackedScene.Instantiate<CustomerCharacter>();
        AddChild(newCustomer);
        newCustomer.GlobalPosition = RetrieveRandomSpawnPosition();
        newCustomer.GoingToTableCustomerState.OnCustomerReachTable += NotifyCustomerReached;
        newCustomer.CustomerEatingState.CustomerFinishedEating += OnCustomerFinishedEating;
        CustomerCount++;
    }

    public void OnCustomerFinishedEating()
    {
        GameManager.Instance.PlayerCharacter.PlayerState.Credit(80);
    }

    private PackedScene CustomerPackedScene { get; set; }

    private CollisionShape3D CustomerSpawnArea { get; set; }

    private BoxShape3D CustomerSpawnAreaShape { get; set; }

    private Timer CustomerSpawnTimer { get; set; }

    public void Initialize()
    {
        RestaurantTables = GetTree().Root
            .GetNode("./SceneRoot/LevelGridMap/FloorsGridMap/NavigationRegion3D/Tables")
            .GetChildren()
            .OfType<RestaurantTable>()
            .ToHashSet();
        RestaurantOrders = new();

        CustomerPackedScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<CustomerCharacter>());

        InitializeCustomerSpawner();
    }

    public void MakeOrder(CustomerCharacter customer)
    {
        RestaurantOrder order = new(
            customer,
            customer.CurrentTable,
            GameManager.Instance.CookingSubsystem.Recipes.First());

        customer.IsAvailableForInteraction = false;

        RestaurantOrders.Add(order);

        OnOrderRequest?.Invoke(order);
    }

    public void OrderFullFiled(RestaurantOrder restaurantOrder)
    {
        restaurantOrder.Customer.StateMachine.ChangeState(restaurantOrder.Customer.CustomerEatingState);

        RestaurantOrders.Remove(restaurantOrder);

        OnOrderFullFiled?.Invoke(restaurantOrder);
    }

    public bool RetrieveAvailableTable(out RestaurantTable availableTable)
    {
        availableTable = null;

        RestaurantTable hasAvailableTable = RestaurantTables.FirstOrDefault(table => table.IsAvailable);

        if (hasAvailableTable is null)
            return false;

        availableTable = hasAvailableTable;
        availableTable.Retrieve();

        return true;
    }

    public Vector3 RetrieveRandomSpawnPosition()
    {
        return CustomerSpawnArea.GlobalPosition +
            new Vector3(
                (float)GD.RandRange(-CustomerSpawnAreaShape.Size.X / 2, CustomerSpawnAreaShape.Size.X / 2),
                0.0f,
                (float)GD.RandRange(-CustomerSpawnAreaShape.Size.Z / 2, CustomerSpawnAreaShape.Size.Z / 2));
    }

    public int CustomerCount { get; set; } = 0;

    public HashSet<RestaurantOrder> RestaurantOrders { get; private set; }

    public HashSet<RestaurantTable> RestaurantTables { get; private set; }
}

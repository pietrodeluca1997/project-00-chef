using Game.Managers;
using Godot;

namespace Project00ChefGame.Scripts.Game.Subsystems.SupplySubsystem;

public partial class SupplySubsystem : Node
{
    private Timer truckCooldown;
    private Timer truckBuyTimer;
    public SupplyTruck Truck { get; private set; }

    public delegate void SupplySubsystemEventSignature();

    public event SupplySubsystemEventSignature OnTruckReachedRestaurantEvent;

    public override void _PhysicsProcess(double delta)
    {
        if (!Truck.Coming || Truck.Arrived) return;

        float deltaf = (float)delta;

        float progressRatio = Truck.TruckPath.ProgressRatio + 0.05f * deltaf;

        if (progressRatio >= 1)
        {
            OnTruckReachedRestaurantEvent?.Invoke();
        }
        else
        {
            Truck.TruckPath.ProgressRatio = progressRatio;
        }
    }

    public override void _Ready()
    {
        Truck = GetTree().Root.GetNode<SupplyTruck>("./SceneRoot/Path3D/PathFollow3D/SupplyTruck");

        truckCooldown = new Timer
        {
            Autostart = true,
            OneShot = true,
            WaitTime = 20.0f
        };

        truckCooldown.Timeout += SpawnSupplyTruck;
        AddChild(truckCooldown);

        OnTruckReachedRestaurantEvent += OnTruckReachedRestaurant;
    }

    public void SpawnSupplyTruck()
    {
        Truck.Coming = true;
        SoundManager.Instance.PlayTruckSoundNotification(Truck.GlobalPosition);
    }

    public void OnTruckReachedRestaurant()
    {
        if (truckBuyTimer is null)
        {
            truckBuyTimer = new Timer
            {
                Autostart = false,
                OneShot = true,
                WaitTime = 20f
            };

            truckBuyTimer.Timeout += DespawnSupplyTruck;
            AddChild(truckBuyTimer);
        }

        truckBuyTimer.Start();

        Truck.Arrive();
    }

    public void DespawnSupplyTruck()
    {
        Truck.GoAway();
        truckCooldown.Start();
    }
}

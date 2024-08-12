using DevelopmentKit.Patterns.Design.Creational;
using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.UI.HUD;
using DevelopmentKit.Worlds;
using Game.Characters.Player;
using Game.PlayerControllers;
using Game.Subsystems.CookingSubystem.Scripts;
using Game.Subsystems.RestaurantSubsystem;
using Game.UI.HUD;
using Godot;
using Project00ChefGame.Scripts.Game.Subsystems.SupplySubsystem;

namespace Game.Managers;

public partial class GameManager : AbstractSingleton<GameManager>
{
    public PlayerCharacter PlayerCharacter { get; private set; }

    public PlayerController PlayerController { get; private set; }

    public CookingSubsystem CookingSubsystem { get; private set; }

    public RestaurantSubsystem RestaurantSubsystem { get; private set; }

    public SupplySubsystem SupplySubsystem { get; private set; }

    public World World { get; private set; }

    public HUDBase HUD { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        LoadWorld();
        LoadSubsystems();
        LoadHud();
        LoadPlayer();
        LoadPlayerController();

        Initialize();
    }

    private void Initialize()
    {
        HUD.Initialize();
        PlayerController.Initialize();
        RestaurantSubsystem.Initialize();
        CookingSubsystem.Initialize();
    }

    private void LoadHud()
    {
        PackedScene hudScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<PlayerHUD>());

        HUD = hudScene.Instantiate<PlayerHUD>();

        AddChild(HUD, true);
    }

    private void LoadPlayer()
    {
        PackedScene playerScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<PlayerCharacter>());

        PlayerCharacter = playerScene.Instantiate<PlayerCharacter>();

        AddChild(PlayerCharacter, true);
    }

    private void LoadPlayerController()
    {
        PlayerController = new PlayerController { PlayerCharacter = PlayerCharacter };

        AddChild(PlayerController, true);
    }

    private void LoadSubsystems()
    {
        RestaurantSubsystem = GetTree().Root.GetNode<RestaurantSubsystem>(nameof(RestaurantSubsystem));
        CookingSubsystem = GetTree().Root.GetNode<CookingSubsystem>(nameof(CookingSubsystem));
        SupplySubsystem = GetTree().Root.GetNode<SupplySubsystem>(nameof(SupplySubsystem));
    }

    private void LoadWorld()
    {
        World = new World();

        AddChild(World, true);
    }
}

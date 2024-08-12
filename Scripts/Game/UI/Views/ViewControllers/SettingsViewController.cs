using DevelopmentKit.Signals;
using DevelopmentKit.Worlds;
using Game.Managers;
using GodotKit.UI.ViewControllers;

namespace Game.UI.Views.ViewControllers;

public partial class SettingsViewController : ViewControllerBase
{
    private bool usingFpsDisplay;
    private WorldSettings worldSettings;

    private void OnDisplayFPSButtonPressed(bool toggledOn) { usingFpsDisplay = toggledOn; }

    public override void _Ready()
    {
        worldSettings = GameManager.Instance.World.WorldSettings;

        SignalConnectionInfo[] signals = new SignalConnectionInfo[]
        {
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/ScreenSpaceAA/OptionButton",
                SignalName = "item_selected",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ChangeScreenSpaceOption)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/MSAA3D/OptionButton",
                SignalName = "item_selected",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ChangeMsaa3DOption)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/TAA/CheckButton",
                SignalName = "toggled",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ToggleTaa)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/Reflections/CheckButton",
                SignalName = "toggled",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ToggleReflections)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/AmbientOcclusion/CheckButton",
                SignalName = "toggled",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ToggleAmbientOcclusion)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/IndirectLightining/CheckButton",
                SignalName = "toggled",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ToggleIndirectLightning)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/GlobalIllumination/CheckButton",
                SignalName = "toggled",
                Instance = worldSettings,
                MethodName = nameof(WorldSettings.ToggleGlobalIllumination)
            },
            new()
            {
                RootNode = this,
                NodeToConnectPath =
                    "./Background/MarginContainer/Container/MarginContainer/HBoxContainer/LeftGraphicsContainer/DisplayFPS/CheckButton",
                SignalName = "toggled",
                Instance = this,
                MethodName = nameof(OnDisplayFPSButtonPressed)
            }
        };

        SignalHelper.Connect(signals);
    }
}

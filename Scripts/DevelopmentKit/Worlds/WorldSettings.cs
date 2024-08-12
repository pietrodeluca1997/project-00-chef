using Godot;

namespace DevelopmentKit.Worlds;

public partial class WorldSettings : Node
{
    private const string Mssaa3DSettingName = "rendering/anti_aliasing/quality/msaa_3d";
    private const string ScreenSpaceAaSettingName = "rendering/anti_aliasing/quality/screen_space_aa";
    private const string TaaSettingName = "rendering/anti_aliasing/quality/use_taa";

    private bool usingTaa, usingReflections, usingAmbientOcclusion, usingIndirectLightining, usingGlobalIllumination;

    private WorldEnvironment worldEnvironment;

    public override void _Ready()
    { worldEnvironment = GetTree().Root.GetNode<WorldEnvironment>("./SceneRoot/WorldEnvironment"); }

    public static void ChangeMsaa3DOption(long index) { ProjectSettings.SetSetting(Mssaa3DSettingName, index); }

    public static void ChangeScreenSpaceOption(long index)
    { ProjectSettings.SetSetting(ScreenSpaceAaSettingName, index); }

    public void ToggleAmbientOcclusion(bool toggledOn)
    {
        usingAmbientOcclusion = toggledOn;
        worldEnvironment.Environment.SsaoEnabled = usingAmbientOcclusion;
    }

    public void ToggleGlobalIllumination(bool toggledOn)
    {
        usingGlobalIllumination = toggledOn;
        worldEnvironment.Environment.SdfgiEnabled = usingGlobalIllumination;
    }

    public void ToggleIndirectLightning(bool toggledOn)
    {
        usingIndirectLightining = toggledOn;
        worldEnvironment.Environment.SsilEnabled = usingIndirectLightining;
    }

    public void ToggleReflections(bool toggledOn)
    {
        usingReflections = toggledOn;
        worldEnvironment.Environment.SsrEnabled = usingReflections;
    }

    public void ToggleTaa(bool toggledOn)
    {
        usingTaa = toggledOn;
        ProjectSettings.SetSetting(TaaSettingName, usingTaa);
    }
}

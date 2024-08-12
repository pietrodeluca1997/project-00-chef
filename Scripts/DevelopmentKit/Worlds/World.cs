using DevelopmentKit.Patterns.Design.Creational;

namespace DevelopmentKit.Worlds;

public partial class World : AbstractSingleton<World>
{
    public override void _Ready()
    {
        WorldSettings = new WorldSettings();

        AddChild(WorldSettings);
    }

    public WorldSettings WorldSettings { get; private set; }
}
using Godot;

namespace DevelopmentKit.Patterns.Design.Creational;

public abstract partial class AbstractSingleton<TSingletonClass> : Node where TSingletonClass : AbstractSingleton<TSingletonClass>
{
    public static TSingletonClass Instance { get; private set; }

    protected AbstractSingleton()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
        }
        else
        {
            Instance = (TSingletonClass)this;
        }
    }
}
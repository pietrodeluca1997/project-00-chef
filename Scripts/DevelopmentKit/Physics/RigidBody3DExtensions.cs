using Godot;
using static Godot.RigidBody3D;

namespace Project00ChefGame.Scripts.DevelopmentKit.Physics;

public static class RigidBody3DExtensions
{
    public static void FreezePhysics(this RigidBody3D rigidBody3D)
    {
        rigidBody3D.FreezeMode = FreezeModeEnum.Static;
        rigidBody3D.Freeze = true;
    }

    public static void UnfreezePhysics(this RigidBody3D rigidBody3D)
    {
        rigidBody3D.FreezeMode = FreezeModeEnum.Kinematic;
        rigidBody3D.Freeze = false;
    }
}

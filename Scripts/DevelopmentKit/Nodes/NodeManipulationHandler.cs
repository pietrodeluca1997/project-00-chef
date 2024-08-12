using Godot;

namespace DevelopmentKit.Nodes;

public class NodeManipulationHandler
{
    public static void Reparent(Node3D node, Node3D newParent)
    {
        if (node != null && newParent != null)
        {
            node.GetParent().RemoveChild(node);
            newParent.AddChild(node);
        }
    }

    public static void RepositionGlobally(Node3D node, Transform3D newGlobalTransform)
    {
        if (node != null)
        {
            node.GlobalTransform = newGlobalTransform;
        }
    }

    public static void RepositionGlobally(Node3D node, Vector3 newGlobalPosition)
    {
        if (node != null)
        {
            node.GlobalPosition = newGlobalPosition;
        }
    }
}

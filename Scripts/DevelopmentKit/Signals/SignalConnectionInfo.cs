using Godot;

namespace DevelopmentKit.Signals;

public readonly record struct SignalConnectionInfo(
    Node RootNode,
    NodePath NodeToConnectPath,
    string SignalName,
    GodotObject Instance,
    string MethodName)
{
}

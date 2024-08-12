using DevelopmentKit.EntityComponents.Interaction;
using Godot;
using System.Collections.Generic;

namespace DevelopmentKit.Signals;

public static class SignalHelper
{
    public static void Connect(SignalConnectionInfo connectionInfo)
    {
        connectionInfo.RootNode
            .GetNode(connectionInfo.NodeToConnectPath)
            .Connect(connectionInfo.SignalName, new Callable(connectionInfo.Instance, connectionInfo.MethodName));
    }

    public static void Connect(IEnumerable<SignalConnectionInfo> connections)
    {
        foreach (SignalConnectionInfo connectionInfo in connections)
        {
            Connect(connectionInfo);
        }
    }

    public static void PrepareInteractableObject<TInteractableClass>(
        TInteractableClass interactableObject,
        NodePath nodePath)
        where TInteractableClass : Node3D, IInteractableObject
    {
        SignalConnectionInfo[] signals = new SignalConnectionInfo[]
        {
            new()
            {
                RootNode = interactableObject,
                NodeToConnectPath = nodePath,
                SignalName = "body_entered",
                Instance = interactableObject,
                MethodName = nameof(IInteractableObject.OnBodyEntered)
            },
            new()
            {
                RootNode = interactableObject,
                NodeToConnectPath = nodePath,
                SignalName = "body_exited",
                Instance = interactableObject,
                MethodName = nameof(IInteractableObject.OnBodyExit)
            }
        };

        Connect(signals);
    }

    public static void PrepareButton(Control component, NodePath buttonPath, string methodName = "OnPressed")
    {
        SignalConnectionInfo signal = new()
        {
            RootNode = component,
            NodeToConnectPath = buttonPath,
            SignalName = "pressed",
            Instance = component,
            MethodName = methodName
        };

        Connect(signal);
    }
}

using Godot;
using System;
using System.Reflection;

namespace DevelopmentKit.Reflection.TypeExtensions;

public static class TypeExtensions
{
    public static TAttributeType GetRequiredAttribute<TAttributeType>(this Type classType) where TAttributeType : Attribute
    {
        TAttributeType attribute = classType.GetCustomAttribute<TAttributeType>() ?? throw new InvalidOperationException($"The type '{classType.Name}' must have the {typeof(TAttributeType).Name} defined.");

        return attribute;
    }

    public static TAttributeType GetAttribute<TAttributeType>(this Type classType) where TAttributeType : Attribute
    {
        TAttributeType attribute = classType.GetCustomAttribute<TAttributeType>();

        if (attribute is null) GD.Print($"The type {classType.Name} doesn't have attribute '{typeof(TAttributeType).Name}' defined.");

        return attribute;
    }
}

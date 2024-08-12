using DevelopmentKit.Reflection.TypeExtensions;
using System;

namespace DevelopmentKit.Reflection.Attributes;

/// <summary>
/// Specifies the path to the scene file associated with a class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ScenePathAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the ScenePathAttribute class with the specified scene path.
    /// </summary>
    /// <param name="path">The path to the scene file.</param>
    public ScenePathAttribute(string path) { Path = path; }

    /// <summary>
    /// Retrieves the scene path from a class marked with ScenePathAttribute.
    /// </summary>
    /// <typeparam name="TClass">The class type to check for the attribute.</typeparam>
    /// <returns>The scene path associated with the class.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the class does not have a ScenePathAttribute.</exception>
    public static string GetPath<TClass>() where TClass : class
    {
        Type classType = typeof(TClass);

        ScenePathAttribute scenePathAttribute = classType.GetRequiredAttribute<ScenePathAttribute>();

        return scenePathAttribute.Path;
    }

    /// <summary>
    /// Gets the path to the scene file.
    /// </summary>
    public string Path { get; }
}

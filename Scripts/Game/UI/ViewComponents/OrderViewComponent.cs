using DevelopmentKit.Reflection.Attributes;
using Godot;
using GodotKit.UI.ViewComponents;

namespace Game.UI.ViewComponents;

[ScenePath("res://UI/ViewComponents/OrderViewComponent.tscn")]
public partial class OrderViewComponent : ViewComponentBase
{
    public Label OrderNameLabel { get; set; }

    public Label TableNumberLabel { get; set; }

    public override void _Ready()
    {
        OrderNameLabel = GetNode<Label>("./OrderBackground/RecipeNameLabel");
        TableNumberLabel = GetNode<Label>("./OrderBackground/HBoxContainer/TableNumberLabel");
    }

}
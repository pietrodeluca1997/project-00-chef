using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.Signals;
using Godot;
using GodotKit.UI.ViewComponents;

namespace Game.UI.ViewComponents;

[ScenePath("res://UI/ViewComponents/CartItemComponent.tscn")]
public partial class CartItemComponent : ViewComponentBase
{
    public delegate void OnItemDecreasedEventSignature(CartItemComponent itemComponent);
    public event OnItemDecreasedEventSignature OnItemDecreased;

    public Label ItemNameLabel { get; set; }

    public Label ItemCountLabel { get; set; }

    public override void _Ready()
    {
        ItemNameLabel = GetNode<Label>("./ItemBackground/HBoxContainer/ItemNameLabel");
        ItemCountLabel = GetNode<Label>("./ItemBackground/HBoxContainer/ItemCount");

        SignalHelper.PrepareButton(this, "./ItemBackground/HBoxContainer/PickButton");
    }

    public void OnPressed()
    {
        OnItemDecreased?.Invoke(this);
    }
}
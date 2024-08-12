using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.Signals;
using Godot;
using GodotKit.UI.ViewComponents;

namespace Game.UI.ViewComponents;

[ScenePath("res://UI/ViewComponents/BuyItemComponent.tscn")]
public partial class BuyItemComponent : ViewComponentBase
{
    public delegate void OnItemBuyedEventSignature(BuyItemComponent itemComponent);
    public event OnItemBuyedEventSignature OnItemBuyed;

    public Label ItemNameLabel { get; set; }
    public Label PriceLabel { get; set; }

    public override void _Ready()
    {
        ItemNameLabel = GetNode<Label>("./ItemBackground/HBoxContainer/ItemNameLabel");
        PriceLabel = GetNode<Label>("./ItemBackground/HBoxContainer2/PriceLabel");

        SignalHelper.PrepareButton(this, "./ItemBackground/HBoxContainer/PickButton");
    }

    public void OnPressed()
    {
        OnItemBuyed?.Invoke(this);
    }
}
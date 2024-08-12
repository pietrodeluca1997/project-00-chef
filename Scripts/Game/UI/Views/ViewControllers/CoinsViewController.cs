using Godot;
using GodotKit.UI.ViewControllers;

namespace Game.UI.Views.ViewControllers;

public partial class CoinsViewController : ViewControllerBase
{
    public Label CoinsLabel { get; set; }

    public override void _Ready()
    {
        base._Ready();
        CoinsLabel = GetNode<Label>("./Background/MarginContainer/HBoxContainer/CoinsLabel");
        UpdateCurrency(100);
    }
    public void UpdateCurrency(int newCurrency)
    {
        CoinsLabel.Text = newCurrency.ToString();
    }
}

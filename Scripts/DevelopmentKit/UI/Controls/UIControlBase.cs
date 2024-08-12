using Godot;

namespace GodotKit.UI.Controls;

public abstract partial class UIControlBase : Control
{
    public virtual void ToggleVisibility() { Visible = !Visible; }
}

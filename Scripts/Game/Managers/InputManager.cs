using DevelopmentKit.Patterns.Design.Creational;
using Game.PlayerControllers;
using Godot;

namespace Game.Managers;

public partial class InputManager : AbstractSingleton<InputManager>
{
    private const string SprintInputName = "sprint";
    private const string DropInputName = "drop";
    private const string InteractInputName = "interact";
    private const string MoveBackwardInputName = "move_backward";
    private const string MoveForwardInputName = "move_forward";
    private const string MoveLeftInputName = "move_left";
    private const string MoveRightInputName = "move_right";
    private const string UiCancelInputName = "ui_cancel";

    public FrameInputValues CurrentFrameInputValues { get; private set; }

    public PlayerController PlayerController { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        CurrentFrameInputValues = new FrameInputValues(
            MovementInput: Input
                .GetVector(MoveLeftInputName, MoveRightInputName, MoveForwardInputName, MoveBackwardInputName),
            OnUiCancel: Input.IsActionJustPressed(UiCancelInputName),
            OnInteractStarted: Input.IsActionJustPressed(InteractInputName),
            OnInteractEnded: Input.IsActionJustReleased(InteractInputName),
            OnDropStarted: Input.IsActionJustPressed(DropInputName),
            OnSprintStarted: Input.IsActionJustPressed(SprintInputName),
            OnSprintEnded: Input.IsActionJustReleased(SprintInputName));

        PlayerController?.OnInputProcess(CurrentFrameInputValues, (float)delta);
    }

    public override void _Ready()
    {
        base._Ready();

        PlayerController = GameManager.Instance.PlayerController;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            PlayerController?.OnMouseMotion(mouseEvent);
        }
    }
}

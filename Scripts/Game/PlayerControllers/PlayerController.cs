using DevelopmentKit.Characters;
using Game.Managers;
using Godot;

namespace Game.PlayerControllers;

public partial class PlayerController : Node
{
    private float MouseSensibility { get; set; }

    public PlayerCharacter3D PlayerCharacter { get; set; }

    public delegate void OnActionEventSignature();

    public event OnActionEventSignature OnUiCancel, OnInteractStarted, OnInteractEnded;

    private void RotateSpringArmByMouseMotion(InputEventMouseMotion mouseEvent)
    {
        PlayerCharacter.SpringArmOffset.RotateY(-mouseEvent.Relative.X * MouseSensibility);
        PlayerCharacter.SpringArm.RotateX(mouseEvent.Relative.Y * MouseSensibility);

        Vector3 currentSpringArmRotation = PlayerCharacter.SpringArm.Rotation;

        PlayerCharacter.SpringArm.Rotation = new Vector3(
            Mathf.Clamp(PlayerCharacter.SpringArm.Rotation.X, -0.55f, Mathf.Pi / 24),
            currentSpringArmRotation.Y,
            currentSpringArmRotation.Z);
    }


    public void CalculateMovementDirection(Vector2 movementDirection, float deltaTime, float speed)
    {
        Vector3 direction = (PlayerCharacter.Transform.Basis * new Vector3(movementDirection.X, 0, movementDirection.Y)).Normalized();

        direction = direction.Rotated(Vector3.Up, PlayerCharacter.SpringArmOffset.Rotation.Y);

        PlayerCharacter.LocomotionComponent.ApplyGroundMovement(direction, deltaTime, speed);
    }

    public void Initialize()
    {
        MouseSensibility = 0.003f;

        Input.WarpMouse(Vector2.Zero);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void OnInputProcess(FrameInputValues inputValues, float deltaTime)
    {
        if (inputValues.OnUiCancel)
            OnUiCancel?.Invoke();

        if (inputValues.OnInteractStarted)
        {
            PlayerCharacter.InteractionComponent.TryInteract();
            OnInteractStarted?.Invoke();
        }

        if (inputValues.OnInteractEnded)
        {
            PlayerCharacter.InteractionComponent.TryStopCurrentInteraction();
            OnInteractEnded?.Invoke();
        }

        if (inputValues.OnDropStarted)
        {
            PlayerCharacter.InteractionComponent.TryDropCarryingObject();
        }
    }

    public void OnMouseMotion(InputEventMouseMotion mouseMotion) { RotateSpringArmByMouseMotion(mouseMotion); }
}

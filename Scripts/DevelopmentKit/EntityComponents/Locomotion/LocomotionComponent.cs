using DevelopmentKit.Characters;
using Godot;

namespace GodotKit.EntityComponents.Locomotion;

public partial class LocomotionComponent : CharacterComponentBase<CharacterBase3D>
{
    [ExportCategory("Movement Properties")]
    [Export] public float Gravity { get; set; } = 50.0f;
    [Export] public float MovementSpeed { get; set; } = 5.0f;
    [Export] public float SprintSpeed { get; set; } = 9.0f;

    private Vector3 targetVelocity = Vector3.Zero;

    public void ApplyGroundMovement(Vector3 movementDirection, float delta, float speed)
    {
        if (movementDirection != Vector3.Zero)
        {
            targetVelocity.X = movementDirection.X * speed;
            targetVelocity.Z = movementDirection.Z * speed;

            Vector3 characterRotation = new(
                OwnerCharacter.CharacterBody.Rotation.X,
                (float)Mathf.LerpAngle(
                    OwnerCharacter.CharacterBody.Rotation.Y,
                    Mathf.Atan2(-targetVelocity.X, -targetVelocity.Z),
                    0.15),
                OwnerCharacter.CharacterBody.Rotation.Z);

            OwnerCharacter.CharacterBody.Rotation = characterRotation;
        }
        else
        {
            targetVelocity.X = Mathf.MoveToward(OwnerCharacter.Velocity.X, 0, speed);
            targetVelocity.Z = Mathf.MoveToward(OwnerCharacter.Velocity.Z, 0, speed);
        }

        if (!OwnerCharacter.IsOnFloor())
        {
            targetVelocity.Y -= Gravity * delta;
        }

        OwnerCharacter.Velocity = targetVelocity;

        OwnerCharacter.MoveAndSlide();
    }
}

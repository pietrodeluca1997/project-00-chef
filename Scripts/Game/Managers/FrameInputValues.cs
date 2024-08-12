using Godot;

namespace Game.Managers;

public readonly record struct FrameInputValues(
    Vector2 MovementInput,
    bool OnUiCancel,
    bool OnInteractStarted,
    bool OnInteractEnded,
    bool OnDropStarted,
    bool OnSprintStarted,
    bool OnSprintEnded);

using Godot;

namespace Game.Subsystems.CookingSubsystem.Contracts;

public interface IUtensilProcess
{
    void Resume();

    void Start();

    void Stop();

    Timer ProcessTimer { get; }
}

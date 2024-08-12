using Game.Subsystems.CookingSubsystem.Contracts;
using Godot;

namespace Game.Subsystems.CookingSubsystem.Entities;

public partial class UtensilProcess : Node3D, IUtensilProcess
{
    public UtensilProcess()
    {
    }

    public UtensilProcess(float waitTime)
    {
        ProcessTimer = new Timer();

        AddChild(ProcessTimer);

        ProcessTimer.WaitTime = waitTime;
        ProcessTimer.OneShot = false;
    }

    public void Resume() { ProcessTimer.Paused = false; }

    public void Start() { ProcessTimer.Start(); }

    public void Stop() { ProcessTimer.Paused = true; }

    public Timer ProcessTimer { get; private set; }
}

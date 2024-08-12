using DevelopmentKit.EntityComponents.Interaction;
using DevelopmentKit.Signals;
using Godot;

namespace Game.Subsystems.CookingSubsystem.Entities;

public abstract partial class UtensilBase : Node3D, IInteractableObject
{
    private Area3D interactionArea;

    [Export]
    public MeshInstance3D MeshInstance { get; set; }

    protected CookingIngredient CurrentIngredientProcessing { get; set; } = null!;

    protected UtensilProcess Process { get; set; }

    [Export]
    protected ProgressBar ProcessProgressBar { get; set; }

    [Export]
    protected MeshInstance3D ProgressBarMesh { get; set; }

    protected float ProcessWaitTime { get; set; }

    protected void CreateProcess()
    {
        Process?.Dispose();

        Process = new UtensilProcess(ProcessWaitTime);
        Process.ProcessTimer.Timeout += ProcessEnded; 
        AddChild(Process);
    }

    protected virtual void ProcessEnded() 
    {
        Process.Dispose();
        ProcessProgressBar.Value = 0;
        ProgressBarMesh.Visible = false;
    }

    protected void StartProcessing(CookingIngredient cookingIngredient)
    {
        if (Process is null || CurrentIngredientProcessing is null || CurrentIngredientProcessing != cookingIngredient)
        {
            CreateProcess();
            Process?.Start();
            CurrentIngredientProcessing = cookingIngredient;
            ProgressBarMesh.Visible = true;
            return;
        }

        if (CurrentIngredientProcessing == cookingIngredient)
        {
            Process.Resume();
            ProgressBarMesh.Visible = true;
        }
    }

    protected void StopProcessing() 
    { 
        Process?.Stop();
        ProgressBarMesh.Visible = false;
    }

    public override void _Ready()
    {
        IsAvailableForInteraction = true;
        interactionArea = GetNode<Area3D>("./Area3D");
        ProgressBarMesh.Visible = false;

        SignalHelper.PrepareInteractableObject(this, "./Area3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        UpdateProgressBar();
    }

    protected void UpdateProgressBar()
    {
        if (ProcessProgressBar != null && Process != null)
        {
            float timeElapsed = (float) (Process.ProcessTimer.WaitTime - Process.ProcessTimer.TimeLeft);
            float totalTime = (float) Process.ProcessTimer.WaitTime;
            float percentageElapsed = (timeElapsed / totalTime) * 100f;

            ProcessProgressBar.Value = percentageElapsed;
        }
    }

    public virtual void Interact(IInteractantActor interactant) { interactant.InteractionComponent.IsOwnerBusy = true; }

    public void OnBodyEntered(Node3D body) => InteractionHandler.HandleBodyEntered(body, this);

    public void OnBodyExit(Node3D body) => InteractionHandler.HandleBodyExited(body, this);

    public virtual void StopInteraction(IInteractantActor interactant)
    { interactant.InteractionComponent.IsOwnerBusy = false; }

    public bool IsAvailableForInteraction { get; private set; }
}

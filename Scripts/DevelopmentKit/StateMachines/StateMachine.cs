namespace DevelopmentKit.StateMachines;

public class StateMachine
{
    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Initialize(IState initialize) { CurrentState = initialize; }

    public void Update(float delta) { CurrentState.Update(delta); }

    public IState CurrentState { get; private set; }
}

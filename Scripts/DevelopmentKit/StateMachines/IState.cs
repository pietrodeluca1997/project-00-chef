namespace DevelopmentKit.StateMachines;

public interface IState
{
    void Enter();

    void Exit();

    void Update(float deltaTime);
}
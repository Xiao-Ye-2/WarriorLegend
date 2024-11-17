public abstract class BaseState
{
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);
    public abstract void OnLogicUpdate();
    public abstract void OnPhysicsUpdate();
    public abstract void OnExit();
}

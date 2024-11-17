public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void OnPhysicsUpdate()
    {
    }

    public override void OnLogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
            return;
        }

        if (!currentEnemy.physicsCheck.isOnGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool(currentEnemy.walk_HASH, false);
        } else
        {
            currentEnemy.anim.SetBool(currentEnemy.walk_HASH, true);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool(currentEnemy.walk_HASH, false);
    }
}

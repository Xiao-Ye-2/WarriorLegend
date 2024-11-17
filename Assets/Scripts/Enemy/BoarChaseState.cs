using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool(currentEnemy.run_HASH, true);
    }
    public override void OnLogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
            return;
        }

        if (!currentEnemy.physicsCheck.isOnGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDirection.x > 0))
        {
            var newLocalScale = new Vector3(currentEnemy.faceDirection.x, currentEnemy.transform.localScale.y, currentEnemy.transform.localScale.z);
            currentEnemy.transform.localScale = newLocalScale;
        }
    }

    public override void OnPhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool(currentEnemy.run_HASH, false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }

}

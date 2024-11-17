public class Boar : Enemy
{
    protected override void Move()
    {
        base.Move();
        anim.SetBool(walk_HASH, true);
    }
}

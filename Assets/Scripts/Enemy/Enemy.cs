using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]
public abstract class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    protected Animator anim;
    protected PhysicsCheck physicsCheck;

    [Header("Basic Settings")]
    public float normalSpeed;
    public float chaseSpeed;

    private float currentSpeed;
    private Vector3 faceDirection;

    [Header("Timer")]
    public float waitTime;
    private float waitTimeCounter;
    private bool wait;

    #region Animator Hashes
    protected readonly int walk_HASH = Animator.StringToHash("walk");
    protected readonly int run_HASH = Animator.StringToHash("run");

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        waitTimeCounter = waitTime;
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        faceDirection = new Vector3(-transform.localScale.x, 0, 0);

        if ((physicsCheck.touchLeftWall && faceDirection.x < 0) || (physicsCheck.touchRightWall && faceDirection.x > 0))
        {
            wait = true;
            anim.SetBool(walk_HASH, false);
            anim.SetBool(run_HASH, false);
        }

        TimeCounter();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        rb.velocity = new Vector2(faceDirection.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    private void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                transform.localScale = new Vector3(faceDirection.x, transform.localScale.y, transform.localScale.z);
                waitTimeCounter = waitTime;
            }
        }
    }
}

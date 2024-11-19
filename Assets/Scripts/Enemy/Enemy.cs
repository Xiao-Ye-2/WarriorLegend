using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]
[RequireComponent(typeof(Character))]
public abstract class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    internal Animator anim;
    internal PhysicsCheck physicsCheck;
    protected Character character;

    [Header("Basic Settings")]
    public float normalSpeed;
    public float chaseSpeed;

    internal float currentSpeed;
    internal Vector3 faceDirection;

    [Header("Timer")]
    public float waitTime;
    private float waitTimeCounter;
    internal bool wait;
    public float lostTime;
    internal float lostTimeCounter;

    [Header("Damage Settings")]
    public float hurtForce;
    private Transform attacker;
    protected bool isHurt;
    protected bool isDead;
    [Header("Detection")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    // Other Internal Variables
    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;


    #region Animator Hashes
    public readonly int walk_HASH = Animator.StringToHash("walk");
    public readonly int run_HASH = Animator.StringToHash("run");
    public readonly int hurt_HASH = Animator.StringToHash("hurt");
    public readonly int dead_HASH = Animator.StringToHash("dead");

    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        character = GetComponent<Character>();

        currentSpeed = normalSpeed;
    }

    private void OnEnable()
    {
        character.OnHurtEvent += OnTakeDamage;
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void OnDisable()
    {
        character.OnHurtEvent -= OnTakeDamage;
        currentState.OnExit();
    }

    private void Update()
    {
        faceDirection = new Vector3(-transform.localScale.x, 0, 0);

        currentState.OnLogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !character.isDead && !wait)
        {
            Move();
        }
        currentState.OnPhysicsUpdate();
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

        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
    }

    internal bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDirection, checkDistance, attackLayer);
    }

    internal void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void OnTakeDamage(Transform attacker)
    {
        this.attacker = attacker;
        if (attacker.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (attacker.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        isHurt = true;
        anim.SetTrigger(hurt_HASH);
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        isDead = true;
        anim.SetBool(dead_HASH, true);
    }

    #region Animation Events
    private void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}

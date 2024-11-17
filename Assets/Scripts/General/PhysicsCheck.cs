using NaughtyAttributes;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;
    [Header("Check Parameters")]
    public bool manual;
    [ShowIf(nameof(manual))] public Vector2 bottomOffset;
    [ShowIf(nameof(manual))] public Vector2 leftOffset;
    [ShowIf(nameof(manual))] public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("Offered Stats")]
    internal bool isOnGround;
    internal bool touchLeftWall;
    internal bool touchRightWall;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2((capsuleCollider.size.x + capsuleCollider.offset.x) * 0.5f, capsuleCollider.size.y * 0.5f);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        isOnGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }
}

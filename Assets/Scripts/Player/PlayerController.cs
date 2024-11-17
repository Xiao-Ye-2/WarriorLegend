using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PhysicsCheck), typeof(Character))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Vector2 inputDirection;
    private Rigidbody2D rb2d;
    private PhysicsCheck physicsCheck;
    private Collider2D coll;
    private Character character;

    [Header("Basic Settings")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    private bool isHurt;
    [Header("Physics Materials")]
    private PhysicsMaterial2D normal;
    public PhysicsMaterial2D noFriction;

    #region Internal Parameters
    public bool isAttack;
    internal bool isDead;

    #endregion

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        coll = GetComponent<Collider2D>();
        character = GetComponent<Character>();
        normal = coll.sharedMaterial;

        inputControl.Gameplay.Jump.performed += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    private void OnEnable()
    {
        character.OnHurtEvent += PlayerHurt;
        character.OnDeadEvent += PlayerDead;
        inputControl.Enable();
    }

    private void OnDisable()
    {
        character.OnHurtEvent -= PlayerHurt;
        character.OnDeadEvent -= PlayerDead;
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (isHurt || isAttack) return;
        Move();
    }

    #region Animation Events

    private void Move()
    {
        rb2d.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb2d.velocity.y);
        float newLocalScaleX = inputDirection.x * transform.localScale.x >= 0 ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(newLocalScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!physicsCheck.isOnGround) return;
        rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!physicsCheck.isOnGround) return;
        EventHandler.CallEvent(nameof(EventHandler.PlayerAttackEvent));
        isAttack = true;
    }

    private void PlayerHurt(Transform attacker)
    {
        isHurt = true;
        rb2d.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        rb2d.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    #endregion

    public void ResetHurt()
    {
        isHurt = false;
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isOnGround ? normal : noFriction;
    }
}

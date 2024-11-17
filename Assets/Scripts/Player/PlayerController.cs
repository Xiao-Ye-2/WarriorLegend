using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PhysicsCheck))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Vector2 inputDirection;
    private Rigidbody2D rb2d;
    private PhysicsCheck physicsCheck;

    [Header("Basic Settings")]
    public float speed;
    public float jumpForce;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();

        inputControl.Gameplay.Jump.performed += Jump;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

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
}

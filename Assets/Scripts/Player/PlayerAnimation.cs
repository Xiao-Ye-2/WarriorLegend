using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(PhysicsCheck))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    [Header("Hashes")]
    private readonly int velocityX_HASH = Animator.StringToHash("velocityX");
    private readonly int velocityY_HASH = Animator.StringToHash("velocityY");
    private readonly int isOnGround_HASH = Animator.StringToHash("isOnGround");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        anim.SetFloat(velocityX_HASH, Math.Abs(rb.velocity.x));
        anim.SetFloat(velocityY_HASH, rb.velocity.y);
        anim.SetBool(isOnGround_HASH, physicsCheck.isOnGround);
    }
}

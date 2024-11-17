using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Character))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private Character character;

    #region Animation String Hash
    private readonly int velocityX_HASH = Animator.StringToHash("velocityX");
    private readonly int velocityY_HASH = Animator.StringToHash("velocityY");
    private readonly int isOnGround_HASH = Animator.StringToHash("isOnGround");
    private readonly int hurt_HASH = Animator.StringToHash("hurt");
    private readonly int dead_HASH = Animator.StringToHash("isDead");
    private readonly int attack_HASH = Animator.StringToHash("attack");
    private readonly int isAttack_HASH = Animator.StringToHash("isAttack");
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        character.OnHurtEvent += PlayHurt;
        EventHandler.PlayerAttackEvent += PlayAttack;
    }

    private void OnDisable()
    {
        character.OnHurtEvent -= PlayHurt;
        EventHandler.PlayerAttackEvent -= PlayAttack;
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
        anim.SetBool(dead_HASH, playerController.isDead);
        anim.SetBool(isAttack_HASH, playerController.isAttack);
    }

    private void PlayHurt(Transform _)
    {
        anim.SetTrigger(hurt_HASH);
    }

    private void PlayAttack()
    {
        anim.SetTrigger(attack_HASH);
    }
}

using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Basic Settings")]
    public float maxHealth;
    public float currentHealth;

    [Header("Invulnerable Settings")]
    public float invulnerableDuration;
    private float invulnerableTimer;
    private bool isInvulnerable;


    private bool isPlayer = false;

    private void Awake()
    {
        isPlayer = GetComponent<PlayerController>() != null;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            if (invulnerableTimer <= 0)
            {
                isInvulnerable = false;
            }
        }
    }

    internal void TakeDamage(Attack attack)
    {
        if (isInvulnerable) return;

        if (currentHealth > attack.damage)
        {
            TriggerInvulnerable();
            currentHealth -= attack.damage;
            if (isPlayer)
            {
                EventHandler.CallEvent(nameof(EventHandler.PlayerHurtEvent), attack.transform);
            }
        }
        else
        {
            currentHealth = 0;
            GetComponent<PlayerController>()?.PlayerDead();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableTimer = invulnerableDuration;
        }
    }
}

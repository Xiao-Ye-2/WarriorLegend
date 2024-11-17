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

        TriggerInvulnerable();
        currentHealth -= attack.damage;
        if (currentHealth <= 0)
        {
            Die();
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

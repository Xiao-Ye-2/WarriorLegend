using System;
using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour
{
    public Action<Transform> OnHurtEvent;
    public Action OnDeadEvent;

    [Header("Basic Settings")]
    public float maxHealth;
    public float currentHealth;

    [Header("Invulnerable Settings")]
    public float invulnerableDuration;
    private float invulnerableTimer;
    private bool isInvulnerable;
    public UnityEvent<Character> OnHealthChange;
    private bool isPlayer = false;
    internal bool isDead;

    private void Awake()
    {
        isPlayer = GetComponent<PlayerController>() != null;
    }

    private void OnEnable()
    {
        OnHurtEvent += TriggerCameraShake;
    }
    private void OnDisable()
    {
        OnHurtEvent -= TriggerCameraShake;
    }

    private void TriggerCameraShake(Transform transform)
    {
        EventHandler.CallEvent(nameof(EventHandler.CameraShakeEvent));
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            currentHealth = 0;
            OnDeadEvent?.Invoke();
            OnHealthChange?.Invoke(this);
        }
    }

    internal void TakeDamage(Attack attack)
    {
        if (isInvulnerable) return;

        if (currentHealth > attack.damage)
        {
            TriggerInvulnerable();
            currentHealth -= attack.damage;
            OnHurtEvent?.Invoke(attack.transform);
        }
        else
        {
            currentHealth = 0;
            OnDeadEvent?.Invoke();
        }

        OnHealthChange?.Invoke(this);
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

using System;
using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour, ISaveable
{
    public Action<Transform> OnHurtEvent;
    public VoidEvent_SO OnNewGameEvent;

    [Header("Basic Settings")]
    public float maxHealth;
    public float currentHealth;

    [Header("Invulnerable Settings")]
    public float invulnerableDuration;
    private float invulnerableTimer;
    private bool isInvulnerable;
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent OnDeadEvent;
    private bool isPlayer = false;
    internal bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        isPlayer = GetComponent<PlayerController>() != null;
    }

    private void OnEnable()
    {
        OnHurtEvent += TriggerCameraShake;
        OnNewGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        OnHurtEvent -= TriggerCameraShake;
        OnNewGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void TriggerCameraShake(Transform transform)
    {
        EventHandler.CallEvent(nameof(EventHandler.CameraShakeEvent));
    }

    private void NewGame()
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
            if (currentHealth <= 0) return;
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

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void SaveGameData(Data data)
    {
        if (data.characterPosDic.ContainsKey(GetDataID().ID))
        {
            data.characterPosDic[GetDataID().ID] = transform.position;
        } else
        {
            data.characterPosDic.Add(GetDataID().ID, transform.position);
        }

        if (data.floatSaveData.ContainsKey(GetDataID().ID + "health"))
        {
            data.floatSaveData[GetDataID().ID + "health"] = currentHealth;
        } else
        {
            data.floatSaveData.Add(GetDataID().ID + "health", currentHealth);
        }
    }

    public void LoadGameData(Data data)
    {
        if (data.characterPosDic.TryGetValue(GetDataID().ID, out Vector3 pos))
        {
            transform.position = pos;
        }
        if (data.floatSaveData.TryGetValue(GetDataID().ID + "health", out float health))
        {
            currentHealth = health;
            OnHealthChange?.Invoke(this);
        }
    }
}

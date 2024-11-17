using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    public CharacterEvent_SO healthEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var healthPercent = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChanged(healthPercent);
    }
}

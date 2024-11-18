using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public SceneLoad_SO loadEvent;
    public PlayerStatBar playerStatBar;
    public CharacterEvent_SO healthEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.loadRequestEvent += OnLoadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.loadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameScene_SO sceneToLoad, Vector3 arg1, bool arg2)
    {
        playerStatBar.gameObject.SetActive(sceneToLoad.type == SceneTypes.Location);
    }

    private void OnHealthEvent(Character character)
    {
        var healthPercent = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChanged(healthPercent);
    }
}

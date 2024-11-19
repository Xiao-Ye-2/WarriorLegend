using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public SceneLoad_SO unloadSceneEvent;
    public PlayerStatBar playerStatBar;
    public VoidEvent_SO loadDataEvent;
    public VoidEvent_SO gameOverEvent;
    public VoidEvent_SO backToMenuEvent;
    public CharacterEvent_SO healthEvent;
    public GameOverPanel gameOverPanel;
    public Button restartBtn;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadSceneEvent.loadRequestEvent += OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadSceneEvent.loadRequestEvent -= OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn.gameObject);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    private void OnUnloadSceneEvent(GameScene_SO sceneToLoad, Vector3 arg1, bool arg2)
    {
        playerStatBar.gameObject.SetActive(sceneToLoad.type == SceneTypes.Location);
    }

    private void OnHealthEvent(Character character)
    {
        var healthPercent = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChanged(healthPercent);
    }
}

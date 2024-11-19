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
    public FloatEvent_SO syncVolumeEvent;
    public VoidEvent_SO backToMenuEvent;
    public VoidEvent_SO pauseGameEvent;
    public CharacterEvent_SO healthEvent;
    public GameOverPanel gameOverPanel;
    public Button restartBtn;
    public Button settingBtn;
    public GameObject settingPanel;
    public Slider volumeSlider;
    public GameObject mobileInputCanvas;

    private void Awake()
    {
        #if UNITY_STANDALONE
            mobileInputCanvas.SetActive(false);
        #endif
        settingBtn.onClick.AddListener(ToggleSettingPanel);
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadSceneEvent.loadRequestEvent += OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadSceneEvent.loadRequestEvent -= OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) * 0.01f;
    }

    private void ToggleSettingPanel()
    {
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
            Time.timeScale = 1;
        } else
        {
            settingPanel.SetActive(true);
            Time.timeScale = 0;
            pauseGameEvent.RaiseEvent();
        }
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

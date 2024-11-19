using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour, ISaveable
{
    public Transform playerTransform;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    public SceneLoad_SO sceneLoadEvent;
    public SceneLoad_SO unLoadSceneEvent;
    public VoidEvent_SO newGameEvent;
    public VoidEvent_SO backToMenuEvent;
    public GameScene_SO  firstLoadScene;
    public GameScene_SO menuScene;

    public VoidEvent_SO afterSceneLoadEvent;
    public FadeEvent_SO fadeEvent;
    private GameScene_SO currentlyLoadedScene;
    private GameScene_SO sceneToGo;
    private Vector3 locationToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;

    private void Start()
    {
        sceneLoadEvent.RaiseEvent(menuScene, menuPosition, true);
    }

    private void OnEnable()
    {
        sceneLoadEvent.loadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        sceneLoadEvent.loadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToGo = menuScene;
        sceneLoadEvent.RaiseEvent(sceneToGo, menuPosition, true);
    }

    private void OnLoadRequestEvent(GameScene_SO scene, Vector3 location, bool fadeScreen)
    {
        if (isLoading) return;

        isLoading = true;
        sceneToGo = scene;
        locationToGo = location;
        this.fadeScreen = fadeScreen;

        if (currentlyLoadedScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        } else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);
        unLoadSceneEvent.RaiseEvent(sceneToGo, locationToGo, true);
        yield return currentlyLoadedScene.sceneReference.UnLoadScene();
        playerTransform.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void NewGame()
    {
        sceneToGo = firstLoadScene;
        sceneLoadEvent.RaiseEvent(sceneToGo, firstPosition, true);
    }

    private void LoadNewScene()
    {
        var op = sceneToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        op.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        currentlyLoadedScene = sceneToGo;

        if (playerTransform)
        {
            playerTransform.position = locationToGo;
        }

        playerTransform.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        if (currentlyLoadedScene.type == SceneTypes.Menu) return;
        afterSceneLoadEvent.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void LoadGameData(Data data)
    {
        var playerID = playerTransform.GetComponent<DataDefinition>().ID;
        if (data.characterPosDic.ContainsKey(playerID))
        {
            sceneToGo = data.GetGameScene();
            OnLoadRequestEvent(sceneToGo, data.characterPosDic[playerID], true);
        }
    }

    public void SaveGameData(Data data)
    {
        data.SaveGameScene(currentlyLoadedScene);
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public SceneLoad_SO sceneLoadEvent;
    public GameScene_SO  firstLoadScene;

    private GameScene_SO currentlyLoadedScene;
    private GameScene_SO sceneToGo;
    private Vector3 locationToGo;
    private bool fadeScreen;
    public float fadeDuration;

    private void Awake()
    {
        currentlyLoadedScene = firstLoadScene;
        currentlyLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        sceneLoadEvent.loadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        sceneLoadEvent.loadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameScene_SO scene, Vector3 location, bool fadeScreen)
    {
        sceneToGo = scene;
        locationToGo = location;
        this.fadeScreen = fadeScreen;

        StartCoroutine(UnloadPreviousScene());
    }

    private IEnumerator UnloadPreviousScene()
    {
        if (fadeScreen)
        {
            yield return new WaitForSeconds(fadeDuration);
        }

        if (currentlyLoadedScene != null)
        {
            yield return currentlyLoadedScene.sceneReference.UnLoadScene();
        }

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        Addressables.LoadSceneAsync(sceneToGo.sceneReference, LoadSceneMode.Additive);
        currentlyLoadedScene = sceneToGo;
    }
}

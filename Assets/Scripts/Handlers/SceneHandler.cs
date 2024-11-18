using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 firstPosition;
    public SceneLoad_SO sceneLoadEvent;
    public GameScene_SO  firstLoadScene;

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
        NewGame();
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
        yield return currentlyLoadedScene.sceneReference.UnLoadScene();
        playerTransform.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void NewGame()
    {
        sceneToGo = firstLoadScene;
        OnLoadRequestEvent(sceneToGo, firstPosition, true);
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
        afterSceneLoadEvent.RaiseEvent();
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SceneLoad_SO", menuName = "ScriptableObjects/SceneLoadEvent")]
public class SceneLoad_SO : ScriptableObject
{
    public UnityAction<GameScene_SO, Vector3, bool> loadRequestEvent;

    public void RaiseEvent(GameScene_SO locationToLoad, Vector3 positionToGo, bool fadeScreen)
    {
        loadRequestEvent?.Invoke(locationToLoad, positionToGo, fadeScreen);
    }
}

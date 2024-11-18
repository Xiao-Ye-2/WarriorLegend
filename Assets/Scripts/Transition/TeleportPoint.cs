using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoad_SO loadEventSO;
    public GameScene_SO sceneToLoad;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        loadEventSO.RaiseEvent(sceneToLoad, positionToGo, true);
    }
}

using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GameScene_SO", menuName = "ScriptableObjects/GameScene")]
public class GameScene_SO : ScriptableObject
{
    public SceneTypes type;
    public AssetReference sceneReference;
}

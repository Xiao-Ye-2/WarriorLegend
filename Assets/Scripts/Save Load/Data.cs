using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    public Dictionary<string, Vector3> characterPosDic = new Dictionary<string, Vector3>();
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public void SaveGameScene(GameScene_SO gameScene)
    {
        sceneToSave = JsonUtility.ToJson(gameScene);
    }

    public GameScene_SO GetGameScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameScene_SO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}

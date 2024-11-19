using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(order: -100)]
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public VoidEvent_SO SaveGameEvent;
    public VoidEvent_SO LoadGameEvent;
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveData = new Data();
    }

    private void OnEnable()
    {
        SaveGameEvent.OnEventRaised += Save;
        LoadGameEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        SaveGameEvent.OnEventRaised -= Save;
        LoadGameEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        if (saveableList.Contains(saveable))
        {
            saveableList.Remove(saveable);
        }
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.SaveGameData(saveData);
        }
    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadGameData(saveData);
        }
    }
}

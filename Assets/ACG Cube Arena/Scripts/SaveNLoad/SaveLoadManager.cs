using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private string saveFilePath;

    public static Action<SaveData> onDataLoaded;

    private SaveData loadedData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        CurrencyManager.onDiamondsChanged += OnDiamondsChangedCallback;
        SettingsManager.onSettingsChange += OnSettingsChangedCallback;
        
    }
    void OnDestroy()
    {
        CurrencyManager.onDiamondsChanged -= OnDiamondsChangedCallback;
        SettingsManager.onSettingsChange -= OnSettingsChangedCallback;
    }

    void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.diamonds = CurrencyManager.instance.GetCurrentDiamonds();
        saveData.bgmVolume = AudioManager.instance.GetBGMVolume();
        saveData.sfxVolume = AudioManager.instance.GetSFXVolume();
        saveData.fullscreenMode = SettingsManager.instance.GetFullscreenMode();
        saveData.resolutionPreset = SettingsManager.instance.GetResolutionPreset();

        Dictionary<StatType, int> unlockedStats = SkillTreeManager.instance.GetSkillLevelsDictionary();
        foreach (var stat in unlockedStats)
        {
            saveData.unlockedStats.Add(stat.Key);
            saveData.unlockedSkillLevel.Add(stat.Value);
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);

            loadedData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.Log("No save data found");
            loadedData = new SaveData();
        }
        onDataLoaded?.Invoke(loadedData);
    }

    private void OnDiamondsChangedCallback(int diamonds)
    {
        SaveGame();
    }

    private void OnSettingsChangedCallback()
    {
        SaveGame();
    }
    
}

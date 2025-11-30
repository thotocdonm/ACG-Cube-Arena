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
        LoadGame();
    }
    void OnDestroy()
    {
        CurrencyManager.onDiamondsChanged -= OnDiamondsChangedCallback;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.diamonds = CurrencyManager.instance.GetCurrentDiamonds();
        saveData.bgmVolume = AudioManager.instance.GetBGMVolume();
        saveData.sfxVolume = AudioManager.instance.GetSFXVolume();

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

            // CurrencyManager.instance.SetDiamonds(saveData.diamonds);
            // AudioManager.instance.SetBGMVolume(saveData.bgmVolume);
            // AudioManager.instance.SetSFXVolume(saveData.sfxVolume);

            // Dictionary<StatType, int> unlockedSkills = new Dictionary<StatType, int>();
            // for (int i = 0; i < saveData.unlockedStats.Count; i++)
            // {
            //     unlockedSkills.Add(saveData.unlockedStats[i], saveData.unlockedSkillLevel[i]);
            // }
            // SkillTreeManager.instance.LoadSkillLevels(unlockedSkills);
        }
        else
        {
            Debug.Log("No save data found");
            loadedData = new SaveData();
            // SkillTreeManager.instance.InitializeSkillLevels();

        }
        onDataLoaded?.Invoke(loadedData);
    }

    private void OnDiamondsChangedCallback(int diamonds)
    {
        SaveGame();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int diamonds;

    public List<StatType> unlockedStats;
    public List<int> unlockedSkillLevel;

    public float bgmVolume;
    public float sfxVolume;

    public SaveData()
    {
        diamonds = 0;
        unlockedStats = new List<StatType>();
        unlockedSkillLevel = new List<int>();
        bgmVolume = 0.1f;
        sfxVolume = 0.1f;
    }
}

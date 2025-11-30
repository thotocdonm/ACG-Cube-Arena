using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("BGMs & SFXs")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource[] allSFXAudioSources;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Player")]
    [SerializeField] private AudioSource[] ichigoAudioSources;
    [SerializeField] private AudioSource ichigoChargingAudioSource;
    [SerializeField] private AudioSource ichigoSkillReleaseAudioSource;
    [SerializeField] private AudioSource playerHittedAudioSource;

    [Header("Enemy")]
    [SerializeField] private AudioSource enemyHitAudioSource;
    [SerializeField] private AudioSource enemySpawnAudioSource;

    [Header("Mage Enemy")]
    [SerializeField] private AudioSource mageChargeAudioSource;
    [SerializeField] private AudioSource mageAttackAudioSource;

    [Header("UI")]
    [SerializeField] private AudioSource buttonHoverAudioSource;
    [SerializeField] private AudioSource statUpgradeAudioSource;

    [Header("Settings")]
    
    private float bgmVolume = 0.1f;
    private float sfxVolume = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        EnemyStats.onEnemyHit += EnemyHitCallback;
        PlayerStats.onPlayerHitted += PlayerHittedCallback;
    }

    void OnEnable()
    {
        SaveLoadManager.onDataLoaded += OnDataLoadedCallback;
    }

    void OnDisable()
    {
        SaveLoadManager.onDataLoaded -= OnDataLoadedCallback;
    }
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
        PlayerStats.onPlayerHitted -= PlayerHittedCallback;
    }

    private void OnDataLoadedCallback(SaveData data)
    {
        SetBGMVolume(data.bgmVolume);
        SetSFXVolume(data.sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudioSource.volume = bgmVolume;
        bgmVolumeSlider.value = bgmVolume;

    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (AudioSource audioSource in allSFXAudioSources)
        {
            audioSource.volume = sfxVolume;
        }
        sfxVolumeSlider.value = sfxVolume;
    }

    public void SaveVolumeSettings()
    {
        SaveLoadManager.instance.SaveGame();
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    private void EnemyHitCallback(int damage, Vector3 enemyPos, bool isCritical, Vector3 hitPoint)
    {
        PlayEnemyHitSound();
    }

    private void PlayerHittedCallback(int damage)
    {
        PlayPlayerHittedSound();
    }

    public void PlayIchigoAttackSound(int index)
    {
        AudioSource audioSource = ichigoAudioSources[index];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    public void PlayIchigoChargingSound()
    {
        
        ichigoChargingAudioSource.Play();
    }

    public void PlayIchigoSkillReleaseSound()
    {
        
        ichigoSkillReleaseAudioSource.Play();
    }

    public void PlayEnemyHitSound()
    {
        enemyHitAudioSource.pitch = Random.Range(0.9f, 1.1f);
        enemyHitAudioSource.Play();
    }

    public void PlayMageChargeSound()
    {
        mageChargeAudioSource.pitch = Random.Range(0.9f, 1.1f);
        mageChargeAudioSource.Play();
    }

    public void PlayMageAttackSound()
    {
        mageAttackAudioSource.pitch = Random.Range(0.9f, 1.1f);
        mageAttackAudioSource.Play();
    }

    public void PlayPlayerHittedSound()
    {
        playerHittedAudioSource.pitch = Random.Range(0.9f, 1.1f);
        playerHittedAudioSource.Play();
    }

    public void PlayEnemySpawnSound()
    {
        enemySpawnAudioSource.pitch = Random.Range(0.9f, 1.1f);
        enemySpawnAudioSource.Play();
    }

    public void PlayButtonHoverSound()
    {
        buttonHoverAudioSource.Play();
    }
    
    public void PlayStatUpgradeSound()
    {
        statUpgradeAudioSource.Play();
    }
}

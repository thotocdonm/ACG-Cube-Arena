using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
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
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
        PlayerStats.onPlayerHitted -= PlayerHittedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnemyHitCallback(int damage, Vector3 enemyPos, bool isCritical, Vector3 hitPoint)
    {
        PlayEnemyHitSound();
    }

    private void PlayerHittedCallback()
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
}

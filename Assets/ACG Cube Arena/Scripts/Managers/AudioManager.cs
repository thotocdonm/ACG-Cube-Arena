using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Elements")]
    [SerializeField] private AudioSource[] ichigoAudioSources;
    [SerializeField] private AudioSource ichigoChargingAudioSource;
    [SerializeField] private AudioSource ichigoSkillReleaseAudioSource;
    [SerializeField] private AudioSource enemyHitAudioSource;

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
    }
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
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
}

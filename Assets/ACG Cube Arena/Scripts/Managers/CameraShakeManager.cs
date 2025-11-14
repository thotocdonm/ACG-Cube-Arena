using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance { get; private set; }

    [Header("Elements")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;




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
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        ResetIntensity();

        EnemyStats.onEnemyHit += EnemyHitCallback;
    }
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
    }

    private void EnemyHitCallback(int damage, Vector3 enemyPos, bool isCritical, Vector3 hitPoint)
    {
        ShakeCamera(1, 0.1f);
    }

    public void ShakeCamera(float intensity, float duration)
    {
        Debug.Log("Camera shake");
        perlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(WaitTime(duration));
    }

    IEnumerator WaitTime(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }
    
    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0;
    }
}

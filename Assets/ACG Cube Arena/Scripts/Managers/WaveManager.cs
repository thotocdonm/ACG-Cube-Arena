using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public enum WaveState { Preparing, WaveInProgress }
    public WaveState CurrentWaveState { get; private set; }

    [Header("Elements")]
    private Transform playerTarget;

    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform enemyParent;

    [Header("Wave Setting")]
    [SerializeField] private int initialEnemyCount = 5;
    [SerializeField] private int maxEnemyCount = 12;
    [SerializeField] private int wavesPerEnemyCount = 5;
    [SerializeField] private int enemyCountIncrease = 2;
    [SerializeField] private float healthMultiplerIncreasePerWave = 0.1f;
    [SerializeField] private float damageMultiplerIncreasePerWave = 0.05f;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnRandomOffset = 20f;

    [Header("UI & Triggers")]
    [SerializeField] private GameObject startWaveTrigger;
    [SerializeField] private GameObject openShopTrigger;
    [SerializeField] private GameObject bossHealthBarUI;
    [SerializeField] private TextMeshProUGUI waveText;

    public int CurrentWave { get; private set; }
    private int currentEnemyCount;

    public static Action<int> onWaveStart;
    public static Action onWaveEnd;




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

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    private void Start()
    {
        UpdateWaveText();
    }

    private void EnterPreparationPhase()
    {
        CurrentWaveState = WaveState.Preparing;
        startWaveTrigger.SetActive(true);
        openShopTrigger.SetActive(true);
        UpdateWaveText();
        Debug.Log("Wave Preparation Phase Started, Enter Circle to continue");
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave " + (CurrentWave + 1).ToString();
    }

    public void StartNextWave()
    {
        if (CurrentWaveState == WaveState.Preparing)
        {
            CurrentWave++;
            StartCoroutine(SpawnWaveCoroutine());
        }

    }

    private IEnumerator SpawnWaveCoroutine()
    {
        CurrentWaveState = WaveState.WaveInProgress;
        startWaveTrigger.SetActive(false);
        openShopTrigger.SetActive(false);
        onWaveStart?.Invoke(CurrentWave);

        if (CurrentWave % 2 == 0)
        {
            currentEnemyCount = 1;
            StartCoroutine(SpawnEnemySequence(bossPrefab, 5));
        }
        else
        {
            currentEnemyCount = initialEnemyCount + (CurrentWave / wavesPerEnemyCount) * enemyCountIncrease;
            for(int i = 0; i < currentEnemyCount; i++)
            {
                StartCoroutine(SpawnEnemySequence(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)],0));
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private IEnumerator SpawnEnemySequence(GameObject enemyPrefab, float indicatorScale)
    {
       Vector3 spawnPoint = GetRandomSpawnPoint();

        // Show Indicator
        GameObject spawnIndicatorInstance = VFXPoolManager.instance.enemySpawnVFXPool.Get();
        spawnIndicatorInstance.transform.position = spawnPoint;
        spawnIndicatorInstance.transform.rotation = Quaternion.identity;
        spawnIndicatorInstance.transform.localRotation = Quaternion.Euler(90, 0, 0);
        if (indicatorScale != 0)
        {
            spawnIndicatorInstance.transform.localScale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
        }
        else
        {
            spawnIndicatorInstance.transform.localScale = Vector3.one;
        }
        DOVirtual.DelayedCall(2f, () => VFXPoolManager.instance.enemySpawnVFXPool.Release(spawnIndicatorInstance));
        yield return new WaitForSeconds(2f);

        //Spawn Enemy
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity, enemyParent);
        enemyInstance.GetComponent<EnemyStats>().ApplyWaveModifier(CurrentWave, healthMultiplerIncreasePerWave, damageMultiplerIncreasePerWave);
        AudioManager.instance.PlayEnemySpawnSound();

        if(enemyPrefab.GetComponent<EnemyStats>().GetBaseStats().enemyType == EnemyType.Boss)
        {
            bossHealthBarUI.SetActive(true);
            bossHealthBarUI.GetComponent<BossHealthBarUI>().Initialize(enemyInstance.GetComponent<EnemyStats>());
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 playerPosition = playerTarget.position;

        Vector2 randomOffset2D = UnityEngine.Random.insideUnitCircle * spawnRandomOffset;
        Vector3 randomOffset = new Vector3(randomOffset2D.x, 0, randomOffset2D.y);

        Vector3 targetPosition = playerPosition + randomOffset;
        targetPosition.y = 0.7f;
        return targetPosition;
    }

    public void OnEnemyDied()
    {
        currentEnemyCount--;
        if (currentEnemyCount <= 0 && CurrentWaveState == WaveState.WaveInProgress)
        {
            EndWave();
        }
    }
    
    private void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    private void EndWave(){
        CurrentWaveState = WaveState.Preparing;
        KillAllEnemies();
        bossHealthBarUI.SetActive(false);
        CoinManager.instance.AddCoins(100);
        EnterPreparationPhase();
    }
}

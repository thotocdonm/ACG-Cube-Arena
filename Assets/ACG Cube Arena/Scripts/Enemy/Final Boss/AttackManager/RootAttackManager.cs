using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttackManager : MonoBehaviour
{
    public static RootAttackManager instance;

    [Header("Settings")]
    [SerializeField] private float rootDuration = 5f;
    [SerializeField] private float rootWindowDuration = 10f;
    [SerializeField] private float damageInterval = 0.1f;
    [SerializeField] private float rootDamageMultiplier = 0.25f;
    [SerializeField] private FinalBossPatternIconSO iconSO;


    private Transform playerTarget;
    private PlayerController playerController;
    private PlayerStats playerStats;
    private GameObject playerObject;
    private bool rootWindowActive;
    private float rootWindowEndTime;
    private EnemyStats stats;
    


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
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
            playerStats = playerObject.GetComponent<PlayerStats>();
        }
    }
    
    void OnEnable()
    {
        GameEventsManager.onPlayerAttackAttempted += OnPlayerAct;
        GameEventsManager.onPlayerSkillAttempted += OnPlayerAct;
    }
    void OnDisable()
    {
        GameEventsManager.onPlayerAttackAttempted -= OnPlayerAct;
        GameEventsManager.onPlayerSkillAttempted -= OnPlayerAct;
    }


    public void ActiveRootWindow(EnemyStats stats)
    {
        Debug.Log("Active Root Window");
        rootWindowActive = true;
        rootWindowEndTime = Time.time + rootWindowDuration;
        this.stats = stats;
        GameEventsManager.TriggerFinalBossPatternStarted(iconSO, rootWindowDuration);
        StartCoroutine(RootWindowRoutine());
    }

    private IEnumerator RootWindowRoutine()
    {
        while (rootWindowActive)
        {
            if (Time.time >= rootWindowEndTime)
            {
                rootWindowActive = false;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator RootDamageRoutine(float duration, EnemyStats stats)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            playerStats.TakeDamage((int)(stats.AttackDamage.GetValue() * rootDamageMultiplier), true);
            yield return new WaitForSeconds(damageInterval);
            elapsed += damageInterval;
        }
        yield return null;
    }
    
    private void OnPlayerAct()
    {
        Debug.LogWarning("Player Act" + rootWindowActive);
        if (!rootWindowActive) return;
        rootWindowActive = false;

        if(playerController != null)
        {
            playerController.EnterRooted(rootDuration);
            StartCoroutine(RootDamageRoutine(rootDuration, stats));
        }
    }
}

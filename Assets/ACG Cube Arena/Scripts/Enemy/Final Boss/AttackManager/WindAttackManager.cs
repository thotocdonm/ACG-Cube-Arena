using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttackManager : MonoBehaviour
{
    public static WindAttackManager instance;



    [Header("Elements")]
    [SerializeField] private float windStrengthMax;
    [SerializeField] private float windStrengthMin;
    [SerializeField] private FinalBossPatternIconSO iconSO;

    private Transform playerTarget;
    private PlayerController playerController;
    private GameObject playerObject;


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
        if(playerObject != null)
        {
            playerTarget = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
        }
    }


    public void StartWindAttackPattern(float duration, EnemyStats stats)
    {
        GameEventsManager.TriggerFinalBossPatternStarted(iconSO, duration);
        StartCoroutine(WindAttackPatternRoutine(duration, stats));
    }


    private IEnumerator WindAttackPatternRoutine(float duration, EnemyStats stats)
    {
        float endTime = Time.time + duration;
        Vector3 windDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
        float windStrength = Random.Range(windStrengthMin, windStrengthMax);
        Debug.Log("Wind Strength: " + windStrength);

        playerController.AddExternalVelocity(windDirection * windStrength);
        yield return new WaitForSeconds(duration);
        playerController.ResetExternalVelocity();
        
        yield return null;
    }
}

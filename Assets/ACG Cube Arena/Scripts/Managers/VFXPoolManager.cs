using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXPoolManager : MonoBehaviour
{
    public static VFXPoolManager instance;

    [Header("Enemy VFXs")]
    [SerializeField] private GameObject enemyAoeIndicator;
    [SerializeField] private GameObject enemyAoeVFX;
    [SerializeField] private GameObject enemyHittedVFX;
    [SerializeField] private GameObject enemySpawnVFX;

    [Header("Player VFXs")]
    [SerializeField] private GameObject playerSlashVFX;

    public ObjectPool<GameObject> enemyAoeIndicatorPool;
    public ObjectPool<GameObject> enemyAoeVFXPool;
    public ObjectPool<GameObject> enemyHittedVFXPool;
    public ObjectPool<GameObject> playerSlashVFXPool;
    public ObjectPool<GameObject> enemySpawnVFXPool;

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

        enemyAoeIndicatorPool = new ObjectPool<GameObject>(()=> Instantiate(enemyAoeIndicator),ActionOnGet,ActionOnRelease,ActionOnDestroy);
        enemyAoeVFXPool = new ObjectPool<GameObject>(()=> Instantiate(enemyAoeVFX),ActionOnGet,ActionOnRelease,ActionOnDestroy);
        enemyHittedVFXPool = new ObjectPool<GameObject>(()=> Instantiate(enemyHittedVFX),ActionOnGet,ActionOnRelease,ActionOnDestroy);
        playerSlashVFXPool = new ObjectPool<GameObject>(() => Instantiate(playerSlashVFX), ActionOnGet, ActionOnRelease, ActionOnDestroy);
        enemySpawnVFXPool = new ObjectPool<GameObject>(() => Instantiate(enemySpawnVFX), ActionOnGet, ActionOnRelease, ActionOnDestroy);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActionOnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    private void ActionOnRelease(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
    private void ActionOnDestroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}

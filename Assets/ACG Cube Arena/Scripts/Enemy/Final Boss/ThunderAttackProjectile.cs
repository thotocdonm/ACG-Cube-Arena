using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThunderAttackProjectile : MonoBehaviour
{

    [Header("Config")]
    [SerializeField] private LayerMask wallMask;
    
    
    private GameObject arenaCenter;
    private int damage;
    private Rigidbody rb;
    private Vector3 lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        arenaCenter = GameObject.FindGameObjectWithTag("ArenaCenter");
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            rb.velocity = rb.velocity.normalized * 15f;
        }
        lastPos = transform.position;
    }


    public void Fire()
    {
        if (rb != null)
        {
            rb.velocity = transform.forward * 15f;
        }
    }

    public void Initialize(int damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        }

        if(((1 << other.gameObject.layer) & wallMask) != 0)
        {
            
            Vector3 dir = rb.velocity.sqrMagnitude > 0.01f ? rb.velocity.normalized : transform.forward;
            float distance = Vector3.Distance(lastPos, transform.position) + 3f;
            if (Physics.Raycast(lastPos, dir, out RaycastHit hit, distance, wallMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 arenaCenterPosition = arenaCenter.transform.position;
                Vector3 directionToArenaCenter = (arenaCenterPosition - hit.point).normalized;
                //Calculate new direction
                Vector3 newDirection = Vector3.Reflect(dir, hit.normal);
                newDirection.y = 0f;

                float dot = Vector3.Dot(newDirection, directionToArenaCenter);

                if(dot <= 0f)
                {
                    newDirection = Vector3.Reflect(newDirection, directionToArenaCenter);
                    newDirection.y = 0f;
                    newDirection = newDirection.normalized;
                }

                transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);

                transform.position = hit.point + hit.normal * 0.1f;
                rb.velocity = newDirection.normalized * 25f;
            }
            
        }
    }

}

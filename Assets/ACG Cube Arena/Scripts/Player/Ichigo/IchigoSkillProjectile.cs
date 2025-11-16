using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IchigoSkillProjectile : MonoBehaviour
{
    private int damage;
    private bool isCritical;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        if(rb != null)
        {
            rb.velocity = transform.forward * 25f;
        }
    }

    public void Initialize(int damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(damage, isCritical, other.ClosestPoint(transform.position));
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float lifeTime;
    float damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(float firePower, float newDamage)
    {
        rb.AddForce(transform.forward * firePower);
        damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}

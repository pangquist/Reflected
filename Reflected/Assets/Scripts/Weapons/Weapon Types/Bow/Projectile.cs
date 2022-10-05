using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField] float lifeTime;
    protected float damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
    }

    public virtual void Fire(float firePower, float newDamage)
    {
        rb.AddForce(transform.forward * firePower);
        damage = newDamage;
    }
}

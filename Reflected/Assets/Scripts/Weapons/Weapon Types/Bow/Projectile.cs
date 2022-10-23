using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected enum projectileType {normal, fire, freeze, explosive}
    protected projectileType currentProjectile;
    protected Rigidbody rb;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected StatusEffectData data;
    protected float damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
        lifeTime = 5;
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

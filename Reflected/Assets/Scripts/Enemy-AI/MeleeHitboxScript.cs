using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxScript : MonoBehaviour
{
    public float upTime = 0.5f;
    private float despawnTimer;
    private bool damageDone;

    private Rigidbody rb;
    public int damageAmount = 5;

    void Start()
    {
        despawnTimer = 0f;
        damageDone = false;
    }

    void Update()
    {
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= upTime)
        {
            Destroy(this.gameObject);
            despawnTimer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("AOE collision triggered");
        if (!damageDone)
        {
            if (other.tag == "Player")
            {
                var healthComponent = other.GetComponent<Player>();
                if (healthComponent != null)
                {
                    Debug.Log("damaged by Melee");
                    damageDone = true;
                    healthComponent.TakeDamage(damageAmount);
                }
            }
        }
    }
}

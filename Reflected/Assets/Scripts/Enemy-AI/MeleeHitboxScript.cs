using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxScript : MonoBehaviour
{
    [SerializeField] private float upTime = 0.5f;
    private float despawnTimer;
    private bool damageDone;

    private Rigidbody rb;

    [SerializeField] float damageAmount = 5f; //Base damage

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

    public void SetUp(float damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!damageDone)
        {
            if (other.tag == "Player")
            {
                var healthComponent = other.GetComponent<Player>();
                if (healthComponent != null)
                {
                    damageDone = true;
                    healthComponent.TakeDamage(damageAmount);
                }
            }
        }
    }
}

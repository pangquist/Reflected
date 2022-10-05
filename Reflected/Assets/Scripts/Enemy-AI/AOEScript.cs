using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEScript : MonoBehaviour
{
    public float upTime = 3f;
    private float despawnTimer;
    private float iFrames = 0.5f;
    private float damageTimer;

    private Rigidbody rb;
    public int damageAmount = 3;

    void Start()
    {
        despawnTimer = 0f;
        damageTimer = 0f;
        //this.GetComponent<MeshRenderer>().material.color = new Color(177f, 66f, 238f, 1f);
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
        damageTimer += Time.deltaTime;
        if (damageTimer >= iFrames)
        {
            if (other.tag == "Player")
            {
                var healthComponent = other.GetComponent<Player>();
                if (healthComponent != null)
                {
                    Debug.Log("damaged by AOE");
                    damageTimer = 0f;
                    healthComponent.TakeDamage(damageAmount);

                }
            }
            damageTimer = 0f;
        }
    }
}

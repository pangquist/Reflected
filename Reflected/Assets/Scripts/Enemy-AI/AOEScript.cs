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
    [SerializeField] private float damageAmount = 1; //Base damage 

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

    public void SetUp(float damageAmount, Vector3 aoeSize)
    {
        this.damageAmount = damageAmount;

        transform.localScale = aoeSize;
    }

    private void OnTriggerStay(Collider other)
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= iFrames)
        {
            if (other.tag == "Player")
            {
                Debug.Log("AOE collision activated. Potential damage: "+ damageAmount);
                var healthComponent = other.GetComponent<Player>();
                if (healthComponent != null)
                {
                    damageTimer = 0f;
                    healthComponent.TakeDamage(damageAmount);
                    Debug.Log("Player took this amount of damage: " + damageAmount + " from aoe.");
                }
            }
            damageTimer = 0f;
        }
    }
}

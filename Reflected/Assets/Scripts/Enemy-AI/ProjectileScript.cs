using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 direction;
    private float projectileForce;

    public float upTime = 1f;
    private float despawnTimer;

    private Rigidbody rb;
    public int damageAmount = 3;

    void Start()
    {
        upTime = 1f;
        despawnTimer = 0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= upTime)
        {
            Destroy(this.gameObject);
            despawnTimer = 0f;
        }

        //If collision with player then do damage and destroy this object.


    }

    public void SetUp(Vector3 target, Vector3 spawnPoint, float projectileForce)
    {
        direction = target - spawnPoint;
        //Debug.Log("Direction " + direction);
        this.projectileForce = projectileForce;
        GetComponent<Rigidbody>().AddForce(direction.normalized * projectileForce, ForceMode.Impulse);
        //Debug.Log("Projectile SetUp entered");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Projectile collision triggered");
        if (other.tag == "Player")
        {
            var healthComponent = other.GetComponent<Player>();
            if (healthComponent != null)
            {
                Debug.Log("damaged by projectile");
                Destroy(this.gameObject);
                healthComponent.TakeDamage(damageAmount);
                
            }

            Destroy(this.gameObject);
        }

        //Destroy(this.gameObject);
    }
}

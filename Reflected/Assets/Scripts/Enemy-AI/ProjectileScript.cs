using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 direction;

    public float upTime = 1f;
    private float despawnTimer;

    [SerializeField] private float damageAmount = 3f; //Default damage

    void Start()
    {
        upTime = 1f;
        despawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= upTime)
        {
            Destroy(gameObject);
            despawnTimer = 0f;
        }
    }

    public void SetUp(Vector3 target, Vector3 spawnPoint, float projectileForce, float damageAmount)
    {
        direction = target - spawnPoint;
        this.damageAmount = damageAmount;
        GetComponent<Rigidbody>().AddForce(direction.normalized * projectileForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var healthComponent = other.GetComponentInChildren<Player>();
            if (healthComponent != null)
            {
                Destroy(this.gameObject);
                healthComponent.TakeDamage(damageAmount);
            }

            Destroy(this.gameObject);
        }
    }
}

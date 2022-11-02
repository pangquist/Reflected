using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float upTime = 0.5f;
    private float despawnTimer;
    private bool playerHit;

    private Rigidbody rb;

    void Start()
    {
        despawnTimer = 0f;
        playerHit = false;
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

    public void SetUp(Vector3 aoeSize)
    {
        transform.localScale = aoeSize;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerHit)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Explosion collision entered");
                //Attach slowing debuf
                //Attach damage over time debuf

                playerHit = true;
            }
        }
    }
}

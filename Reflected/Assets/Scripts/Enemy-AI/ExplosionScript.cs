using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float upTime = 0.5f;
    [SerializeField] private StatusEffectData dotData;
    [SerializeField] private StatusEffectData slowData;
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

    public void SetUp(Vector3 aoeSize, StatusEffectData dotData, StatusEffectData slowData)
    {
        transform.localScale = aoeSize;
        this.dotData = dotData;
        this.slowData = slowData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerHit)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Explosion collider entered");

                var healthComponent = other.GetComponent<Player>();
                if (healthComponent != null)
                {
                    Debug.Log("Test");
                    healthComponent.ApplyEffect(dotData, 1);
                    healthComponent.ApplyEffect(slowData, 10);
                    Debug.Log("Effects applied");
                }

                playerHit = true;
            }
        }
    }
}

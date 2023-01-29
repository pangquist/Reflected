using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float upTime = 0.25f;
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
            Destroy(transform.parent.gameObject);
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
                var effectable = other.GetComponentInChildren<IEffectable>();
                if (effectable != null)
                {
                    effectable.ApplyEffect(dotData, 1);
                    effectable.ApplyEffect(slowData, 2);
                    //Debug.Log("Effects applied to player");
                }

                playerHit = true;
            }
        }
    }
}

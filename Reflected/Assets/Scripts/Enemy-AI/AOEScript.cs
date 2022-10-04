using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEScript : MonoBehaviour
{
    public float upTime = 3f;
    private float despawnTimer;
    void Start()
    {
        despawnTimer = 0f;
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
}

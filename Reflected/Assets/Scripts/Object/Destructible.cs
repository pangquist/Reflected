using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    private bool beingDestroyed;

    private void Awake()
    {
        MapGenerator.Finished.AddListener(EnableCollider);
    }

    private void EnableCollider()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void DestroyAnimation()
    {
        if (beingDestroyed)
            return;

        // Play animation that destroys the object instead of calling Destroy(gameObject).
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().Play("Destroy");
            beingDestroyed = true;
        }
        else
            Destroy(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    bool beingDestroyed;
    public void DestroyAnimation()
    {
        if (beingDestroyed)
            return;
        //Play animation that destroys the object instead of calling Destroy(gameObject).
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

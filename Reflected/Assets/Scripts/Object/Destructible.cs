using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int hitsToDestroy = 1;

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

        Animator animator = GetComponent<Animator>();

        hitsToDestroy--;

        if (hitsToDestroy > 0)
        {
            // Play damaged animation
            animator?.Play("Damage");
        }

        else
        {
            beingDestroyed = true;

            // Play animation that destroys the object instead of calling Destroy(gameObject).
            if (animator)
                animator.Play("Destroy");
            else
                Destroy(gameObject);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

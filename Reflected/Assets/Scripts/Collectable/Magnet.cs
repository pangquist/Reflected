using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerStay(Collider other) //If every collectable is supposed to move then as SetTarget to ICollectable
    {
        if(other.gameObject.TryGetComponent<IMagnetic>(out IMagnetic coin))
        {
            coin.SetTarget(transform.parent.position);
        }

    }
}
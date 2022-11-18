using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] private StatusEffectData data;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        var effectable = collider.GetComponentInChildren<IEffectable>();
        if (effectable != null)
        {
            effectable.ApplyEffect(data, 1);
        }
    }
}

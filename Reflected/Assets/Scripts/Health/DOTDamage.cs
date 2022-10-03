using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTDamage : MonoBehaviour
{
    [SerializeField] private StatusEffectData data;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        var effectable = collider.GetComponent<IEffectable>();
        if (effectable != null)
        {
            effectable.ApplyEffect(data);
        }
    }
}

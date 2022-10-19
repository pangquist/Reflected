using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatusEffect : MonoBehaviour
{
    public void ApplyEffectToTarget(Collider collider, StatusEffectData data)
    {
        var effectable = collider.GetComponent<IEffectable>();
        if (effectable != null)
        {
            effectable.ApplyEffect(data, 1);
        }
    }
}

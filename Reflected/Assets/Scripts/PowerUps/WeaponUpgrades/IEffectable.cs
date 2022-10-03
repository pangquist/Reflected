using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyEffect(StatusEffectData data);
    public void RemoveEffect(StatusEffect status);

    public void HandleEffect();

    
}



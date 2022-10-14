using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyEffect(StatusEffectData data);
    public void RemoveEffect(Effect status);

    public void HandleEffect();

    
}



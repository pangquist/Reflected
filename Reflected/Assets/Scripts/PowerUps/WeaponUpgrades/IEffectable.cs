using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyEffect(StatusEffectData data, float scale);
    public void RemoveEffect(Effect status);


    public void HandleEffect(); 
}



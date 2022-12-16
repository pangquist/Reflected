using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialDummy : Character
{
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        base.Awake();
    }

    void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        currentHealth += damage;
    }
}

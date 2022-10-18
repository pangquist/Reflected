using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewPew : Ability
{
    [SerializeField] Bow bow;

    void Start()
    {
        
    }

    public override bool DoEffect()
    {
        base.DoEffect();

        

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UppgradeHolder : MonoBehaviour
{
    public Uppgrade uppgrade;
    float cooldownTime;
    float activeTime;

    enum UppgradeState
    {
        ready,
        active,
        cooldown
    }

    UppgradeState state = UppgradeState.ready;
    public KeyCode key;

    private void Update()
    {
        switch (state)
        {
            case UppgradeState.ready:
                if (Input.GetKeyDown(key))
                {
                    uppgrade.Active();
                    state = UppgradeState.active;
                    activeTime = uppgrade.avtiveTime;
                }
                break;
            case UppgradeState.active:
                if(activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    state = UppgradeState.cooldown;
                    cooldownTime = uppgrade.cooldownTime;
                }                
                break;
            case UppgradeState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = UppgradeState.ready;
                }
                break;
        }
    }
}

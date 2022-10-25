using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    [Header("Dash Specific")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashSpeed;
    [SerializeField] GameObject parent;
    bool isDashing;

    public override bool DoEffect()
    {
        if (IsOnCooldown())
            return false;

        base.DoEffect();

        cooldownstarter.Ability2Use();
        StartCoroutine(dashAction());

        return true;
    }

    IEnumerator dashAction()
    {
        isDashing = true;
        float progress = 0;
        while (progress < dashDuration)
        {
            parent.transform.position += transform.forward * dashSpeed;
            progress += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        yield break;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}

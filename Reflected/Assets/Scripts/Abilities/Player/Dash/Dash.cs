using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    [Header("Dash Specific")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashSpeed;
    bool isDashing;

    public override void DoEffect()
    {
        if (IsOnCooldown())
            return;

        base.DoEffect();

        cooldownstarter.Ability2Use();
        StartCoroutine(dashAction());
    }

    IEnumerator dashAction()
    {
        isDashing = true;
        float progress = 0;
        while (progress < dashDuration)
        {
            transform.position += transform.forward * dashSpeed;
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

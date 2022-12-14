using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    [Header("Dash Specific")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashSpeed;
    [SerializeField] GameObject parent;
    PlayerController playerController;
    bool isDashing;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

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
        playerController.MovementLock();
        float progress = 0;
        while (progress < dashDuration && isDashing)
        {
            parent.transform.position += transform.forward * dashSpeed;
            progress += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        playerController.MovementUnlock();
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDashing)
            return;

        if (other.GetComponent<Enemy>() || other.GetComponent<tutorialDummy>())
        {
            //if (other.GetComponent<Boss>() || other.GetComponent<Root>())
            //    return;

            isDashing = false;
            GetComponent<Animator>().Play("Headbutt");

            //Make a new dash that damages the enemy and remove this (can be a power up or upgrade?)
            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(GetComponent<Player>().GetDamage());
            else if(other.GetComponent<tutorialDummy>())
                other.GetComponent<tutorialDummy>().TakeDamage(GetComponent<Player>().GetDamage());
        }
    }

    public bool IsDashing() => isDashing;
}

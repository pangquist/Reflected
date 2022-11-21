//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slam description
/// </summary>
public class Slam : Ability
{
    [SerializeField] GameObject hitboxObject;
    [SerializeField] Bounds bounds;
    [SerializeField] float duration;
    [SerializeField] float stunDuration;

    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;

    public override bool DoEffect()
    {
        base.DoEffect();

        GetComponent<Animator>().Play("Slam");

        return true;
    }

    public void StartSlamAttack()
    {
        StartCoroutine(Ability());
    }

    IEnumerator Ability()
    {
        hitboxObject.transform.localScale = startScale;

        hitboxObject.SetActive(true);
        float progress = 0;

        float rate = 1 / duration;

        while(progress < 1)
        {
            hitboxObject.transform.localScale = Vector3.Lerp(startScale, endScale, progress);

            progress += rate * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    public void SlamAttack()
    {
        bounds = hitboxObject.GetComponent<Collider>().bounds;

        if (hitboxObject.GetComponent<Collider>().bounds.Intersects(player.Hitbox().bounds))
        {
            Debug.Log("Slam Hit! Damage: " + damage + " Player Bounds: " + player.Hitbox().bounds + " Slam Bounds: " + hitboxObject.GetComponent<Collider>().bounds);
            player.TakeDamage(damage);

            player.Stun(stunDuration);
        }

        hitboxObject.SetActive(false);
    }
}
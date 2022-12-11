using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFrenzy : Ability
{
    [SerializeField] List<Collider> hitboxes;
    [SerializeField] Bounds bounds;
    [SerializeField] float duration;
    [SerializeField] float stunDuration;

    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;

    [SerializeField] LayerMask groundMask;

    private void Awake()
    {
        foreach(Root root in GetComponent<Boss>().Roots())
        {
            hitboxes.Add(root.SwipeHitbox());
        }
    }

    public override bool DoEffect()
    {
        base.DoEffect();

        GetComponent<Boss>().RootFrenzy();

        return true;
    }


    public void Frenzy(Root root)
    {
        bounds = root.SwipeHitbox().bounds;

        if (bounds.Intersects(player.Hitbox().bounds))
        {
            player.TakeDamage(damage);

            player.Stun(stunDuration);
        }
        ParticleSystem particleSystem = Instantiate(vfxObject, root.SwipeHitbox().transform.position, root.SwipeHitbox().transform.rotation).GetComponent<ParticleSystem>();
        particleSystem.transform.parent = null;

        root.gameObject.GetComponent<AudioSource>().PlayOneShot(abilitySounds[Random.Range(0, abilitySounds.Count)]);


        RaycastHit hit;
        if (Physics.Raycast(particleSystem.transform.position + new Vector3(0, 2, 0), Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.material = hit.transform.gameObject.GetComponent<Renderer>().material;
        }
    }


}

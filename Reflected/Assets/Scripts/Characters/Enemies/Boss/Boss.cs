//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss description
/// </summary>
public class Boss : Enemy
{
    [Header("Boss Specifics")]
    bool aggroed = false;
    [SerializeField] GameObject rotateBody;
    [SerializeField] float aggroRange;

    Ability lastAbility;
    [SerializeField] List<Ability> abilities;
    [SerializeField] float abilityTimer;
    [SerializeField] List<Root> roots;
    [SerializeField] Canvas healthBarCanvas;

    bool abilityLock;
    bool rotateLock;
    CameraManager cameraManager;
    [SerializeField] Transform cameraFocusPoint;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] Transform lootDropTransform;

    protected override void Update()
    {
        cameraFocusPoint.transform.position = (transform.position + player.transform.position) / 2;
        
        if (isDead)
            return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < aggroRange && !aggroed)
        {
            Activate();
        }

        if (!aggroed)
            return;

        AbilityTimer();
        if (!rotateLock)
            StartCoroutine(_RotateTowardsPlayer());


        base.Update();
    }

    IEnumerator _RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - rotateBody.transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        while(Quaternion.Angle(rotateBody.transform.rotation, targetRotation) > 0.01f && !isDead)
        {
            //Quaternion nextRotation = Quaternion.Lerp(rotateBody.transform.rotation, targetRotation, Time.deltaTime);
            rotateBody.transform.rotation = Quaternion.RotateTowards(rotateBody.transform.rotation, targetRotation, Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

    public void AbilityTimer()
    {
        if (!abilityLock)
            abilityTimer -= Time.deltaTime;

        while (abilityTimer <= 0)
        {
            //Do Random Ability
            Ability chosenAbility = abilities[Random.Range(0, abilities.Count)];
            if (!chosenAbility.IsOnCooldown() && chosenAbility != lastAbility)
            {
                chosenAbility.DoEffect();
                //lastAbility = chosenAbility;
                abilityTimer = chosenAbility.GetCastTime();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void ToggleRotationLock() => rotateLock = !rotateLock;

    public override void LootDrop(Transform lootDropPosition)
    {
        LootDropList = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>().GetCollectablePool();

        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 1, 0);
        Instantiate(LootDropList.GetItem(LootDropList.Count - 1), spawnPosition, Quaternion.Euler(0, 0, 0));
    }
    
    public void RemoveRoot(Root root)
    {
        roots.Remove(root);

        if (roots.Count == 0)
        {
            Die();
        }
    }

    public override void TakeDamage(float damage)
    {
        GetComponent<AudioSource>().PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count)]);
        //Keep this
    }

    protected override void Die()
    {
        base.Die();
        GetComponent<AudioSource>().PlayOneShot(deathSFX);
        LootDrop(lootDropTransform);
        //cameraManager.FocusOnPlayer();
    }



    public override void ToggleInvurnable()
    {
        abilityLock = !abilityLock;

        foreach(Root root in roots)
        {
            root.ToggleInvurnable();
        }
    }

    public void Activate()
    {
        anim.Play("Activation");
        aggroed = true;
        healthBarCanvas.gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
        //cameraManager.NewFocus(cameraFocusPoint);
        //cameraManager.BossSettings();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    [SerializeField] protected float maxHealthIncrease;
    [SerializeField] protected float damageReduction;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float damageIncrease;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float areaOfEffect;
    [SerializeField] protected float cooldownDecrease;
    [SerializeField] protected int chargesToSwapTrue;
    [SerializeField] protected int chargesToSwapMirror;

    //List<string> statNames = new List<string> { "Health, Damage Reduction, Movement Speed, Damage, Attack Speed, AoE, Charges To Swap" };
    public void AddMaxHealth(float amount)
    {
        maxHealthIncrease += amount;
    }

    public void AddDamageReduction(float amount)
    {
        damageReduction *= (1 - amount);
    }

    public void AddMovementSpeed(float amount)
    {
        movementSpeed += amount;
    }

    public void AddDamageIncrease(float amount)
    {
        damageIncrease += amount;
    }

    public void AddAttackSpeed(float amount)
    {
        attackSpeed *= (1 - amount);
    }

    public void AddAreaOfEffect(float amount)
    {
        areaOfEffect += amount;
    }

    public void AddCooldownDecrease(float amount)
    {
        cooldownDecrease *= (1 - amount);
    }

    public void ChangeChargesToSwapTrue(int amount)
    {
        chargesToSwapTrue += amount;
    }

    public void ChangeChargesToSwapMirror(int amount)
    {
        chargesToSwapMirror += amount;
    }


    public void ResetStats()
    {
        //Should be called upon death
        maxHealthIncrease = 0;
        damageReduction = 1;
        movementSpeed = 1;
        damageIncrease = 1;
        attackSpeed = 1;
        areaOfEffect = 1;
        attackSpeed = 1;
        cooldownDecrease = 0;
        chargesToSwapTrue = 0;
        chargesToSwapMirror = 0;
        
    }

    public float GetMaxHealthIncrease() => maxHealthIncrease;
    public float GetDamageReduction() => damageReduction;
    public float GetMovementSpeed() => movementSpeed;
    public float GetDamageIncrease() => damageIncrease;
    public float GetAttackSpeed() => attackSpeed;
    public float GetAreaOfEffect() => areaOfEffect;
    public float GetCooldownDecrease() => cooldownDecrease;
    public int GetChargesToSwapTrue() => chargesToSwapTrue;
    public int GetChargesToSwapMirror() => chargesToSwapMirror;

   
}

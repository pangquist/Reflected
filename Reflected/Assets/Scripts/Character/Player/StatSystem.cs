using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour, ISavable
{
    [SerializeField] float maxHealthIncrease;
    [SerializeField] float damageReduction;
    [SerializeField] float movementSpeed;
    [SerializeField] float damageIncrease;
    [SerializeField] float attackSpeed;
    [SerializeField] float areaOfEffect;

    public void ResetStats()
    {
        //Should be called upon death
        maxHealthIncrease = 0;
        damageReduction = 0;
        movementSpeed = 0;
        damageIncrease = 0;
        attackSpeed = 0;
        areaOfEffect = 0;
    }

    public float GetMaxHealthIncrease() => maxHealthIncrease;
    public float GetDamageReduction() => damageReduction;
    public float GetMovementSpeed() => movementSpeed;
    public float GetDamageIncrease() => damageIncrease;
    public float GetAttackSpeed() => attackSpeed;
    public float GetAreaOfEffect() => areaOfEffect;

    public void AddMaxHealth(float amount)
    {
        maxHealthIncrease += amount;
    }

    public void AddDamageReduction(float amount)
    {
        damageReduction += amount;
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
        attackSpeed += amount;
    }

    public void AddAreaOfEffect( float amount)
    {
        areaOfEffect += amount;
    }

    public object SaveState()
    {
        return new SaveData()
        {
            maxHealthIncrease = this.maxHealthIncrease,
            damageIncrease = this.damageIncrease,
            movementSpeed = this.movementSpeed,
            damageReduction = this.damageReduction,
            attackSpeed = this.attackSpeed,
            areaOfEffect = this.areaOfEffect
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        maxHealthIncrease = saveData.maxHealthIncrease;
        damageIncrease = saveData.damageIncrease;
        movementSpeed = saveData.movementSpeed;
        damageReduction = saveData.damageReduction;
        attackSpeed = saveData.attackSpeed;
        areaOfEffect = saveData.areaOfEffect;
    }

    [Serializable]
    private struct SaveData
    {
        public float maxHealthIncrease;
        public float damageReduction;
        public float movementSpeed;
        public float damageIncrease;
        public float attackSpeed;
        public float areaOfEffect;
    }
}

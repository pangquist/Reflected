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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public float GetMaxHealthIncrease()
    {
        return maxHealthIncrease;
    }

    public void AddMaxHealth(float amount)
    {
        maxHealthIncrease += amount;
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

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

    List<string> statNames = new List<string> { "Health, Damage Reduction, Movement Speed, Damage, Attack Speed, AoE" };

    Player player;
    UpgradeManager upgradeManager;

    private void Start()
    {
        player = GetComponent<Player>();
        try
        {
            upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        }
        catch(Exception e)
        {

        }
        ResetStats();

        if (upgradeManager)
            GetLightStats();
    }

    public void GetLightStats()
    {
        ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetLightPieces();

        foreach(KeyValuePair<string, float> pair in stats)
        {
            if (pair.Key == "Damage")
            {
                AddDamageIncrease(pair.Value);
            }
            else if (pair.Key == "Damage Reduction")
            {
                AddDamageReduction(pair.Value);
            }
            else if (pair.Key == "Movement Speed")
            {
                AddMovementSpeed(pair.Value);
            }
            else if (pair.Key == "Health")
            {
                AddMaxHealth(pair.Value);
            }
            else if (pair.Key == "Attack Speed")
            {
                AddAttackSpeed(pair.Value);
            }
            else if (pair.Key == "AoE")
            {
                AddAreaOfEffect(pair.Value);
            }
        }
    }

    public void GetDarkStats()
    {
        ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetDarkPieces();

        foreach (KeyValuePair<string, float> pair in stats)
        {
            if (pair.Key == "Damage")
            {
                AddDamageIncrease(pair.Value);
            }
            else if (pair.Key == "Damage Reduction")
            {
                AddDamageReduction(pair.Value);
            }
            else if (pair.Key == "Movement Speed")
            {
                AddMovementSpeed(pair.Value);
            }
            else if (pair.Key == "Health")
            {
                AddMaxHealth(pair.Value);
            }
            else if (pair.Key == "Attack Speed")
            {
                AddAttackSpeed(pair.Value);
            }
            else if (pair.Key == "AoE")
            {
                AddAreaOfEffect(pair.Value);
            }
        }
    }

    public void ResetStats()
    {
        //Should be called upon death
        maxHealthIncrease = 1;
        damageReduction = 1;
        movementSpeed = 1;
        damageIncrease = 1;
        attackSpeed = 1;
        areaOfEffect = 1;
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

    public void AddAreaOfEffect(float amount)
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

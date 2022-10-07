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
    [SerializeField] int chargesToSwapTrue;
    [SerializeField] int chargesToSwapMirror;

    //List<string> statNames = new List<string> { "Health, Damage Reduction, Movement Speed, Damage, Attack Speed, AoE, Charges To Swap" };

    Player player;
    UpgradeManager upgradeManager;

    private void Start()
    {
        player = GetComponent<Player>();
        try
        {
            upgradeManager = GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>();
        }
        catch (Exception e)
        {

        }

        GetLightStats();
    }

    public void GetLightStats()
    {
        if (!upgradeManager)
            return;

        ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetTrueNodes();

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
            else if(pair.Key == "True Charges")
            {
                ChangeChargesToSwapTrue((int)pair.Value);
            }
            else if (pair.Key == "Mirror Charges")
            {
                ChangeChargesToSwapMirror((int)pair.Value);
            }
        }
    }

    public void GetDarkStats()
    {
        if (!upgradeManager)
            return;

        ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetMirrorNodes();

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
            else if (pair.Key == "True Charges")
            {
                ChangeChargesToSwapTrue((int)pair.Value);
            }
            else if (pair.Key == "Mirror Charges")
            {
                ChangeChargesToSwapMirror((int)pair.Value);
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
        chargesToSwapTrue = 0;
        chargesToSwapMirror = 0;
        attackSpeed = 1;
    }

    public float GetMaxHealthIncrease() => maxHealthIncrease;
    public float GetDamageReduction() => damageReduction;
    public float GetMovementSpeed() => movementSpeed;
    public float GetDamageIncrease() => damageIncrease;
    public float GetAttackSpeed() => attackSpeed;
    public float GetAreaOfEffect() => areaOfEffect;
    public int GetChargesToSwapTrue() => chargesToSwapTrue;
    public int GetChargesToSwapMirror() => chargesToSwapMirror;

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

    public void ChangeChargesToSwapTrue(int amount)
    {
        chargesToSwapTrue += amount;
    }

    public void ChangeChargesToSwapMirror(int amount)
    {
        chargesToSwapMirror += amount;
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

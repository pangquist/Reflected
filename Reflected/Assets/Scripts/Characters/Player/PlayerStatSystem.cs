using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatSystem : StatSystem //, ISavable
{
    Player player;
    [SerializeField] UpgradeManager upgradeManager;

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

        //ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetTrueNodes();
        SubtractStats(upgradeManager.GetMirrorNodes());

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
            else if (pair.Key == "Cooldown")
            {
                AddCooldownDecrease(pair.Value);
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

    public void SubtractStats(Dictionary<string, float> statsToRemove)
    {
        foreach (KeyValuePair<string, float> pair in statsToRemove)
        {
            if (pair.Key == "Damage")
            {
                SubtractDamageIncrease(pair.Value);
            }
            else if (pair.Key == "Damage Reduction")
            {
                SubtractDamageReduction(pair.Value);
            }
            else if (pair.Key == "Movement Speed")
            {
                SubtractMovementSpeed(pair.Value);
            }
            else if (pair.Key == "Health")
            {
                SubtractMaxHealth(pair.Value);
            }
            else if (pair.Key == "Attack Speed")
            {
                SubtractAttackSpeed(pair.Value);
            }
            else if (pair.Key == "AoE")
            {
                SubtractAreaOfEffect(pair.Value);
            }
            else if (pair.Key == "Cooldown")
            {
                SubtractCooldownDecrease(pair.Value);
            }
            else if (pair.Key == "True Charges")
            {
                SubtractChangeChargesToSwapTrue((int)pair.Value);
            }
            else if (pair.Key == "Mirror Charges")
            {
                SubtractChangeChargesToSwapMirror((int)pair.Value);
            }
        }
    }

    public void GetDarkStats()
    {
        if (!upgradeManager)
            return;

        //ResetStats();

        Dictionary<string, float> stats = upgradeManager.GetMirrorNodes();
        SubtractStats(upgradeManager.GetTrueNodes());

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
            else if (pair.Key == "Cooldown")
            {
                AddCooldownDecrease(pair.Value);
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

    

    //public object SaveState()
    //{
    //    return new SaveData()
    //    {
    //        maxHealthIncrease = this.maxHealthIncrease,
    //        damageIncrease = this.damageIncrease,
    //        movementSpeed = this.movementSpeed,
    //        damageReduction = this.damageReduction,
    //        attackSpeed = this.attackSpeed,
    //        areaOfEffect = this.areaOfEffect,
    //        cooldownDecrease = this.cooldownDecrease
    //    };
    //}

    //public void LoadState(object state)
    //{
    //    var saveData = (SaveData)state;
    //    maxHealthIncrease = saveData.maxHealthIncrease;
    //    damageIncrease = saveData.damageIncrease;
    //    movementSpeed = saveData.movementSpeed;
    //    damageReduction = saveData.damageReduction;
    //    attackSpeed = saveData.attackSpeed;
    //    areaOfEffect = saveData.areaOfEffect;
    //    cooldownDecrease = saveData.cooldownDecrease;
    //}

    //[Serializable]
    //private struct SaveData
    //{
    //    public float maxHealthIncrease;
    //    public float damageReduction;
    //    public float movementSpeed;
    //    public float damageIncrease;
    //    public float attackSpeed;
    //    public float areaOfEffect;
    //    public float cooldownDecrease;
    //}
}

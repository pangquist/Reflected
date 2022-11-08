using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyStatSystem : StatSystem
{
    Stats stat;
    Dictionary<Stats, float> lightBuffs, darkBuffs, baseBuffs;
    List<Stats> statList;
    List<float> baseIncrease;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void Start()
    {
        baseIncrease = new List<float>() { 2f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f};
        statList = new List<Stats>();
        statList = Enum.GetValues(typeof(Stats)).Cast<Stats>().ToList();
        lightBuffs = new Dictionary<Stats, float>();
        darkBuffs = new Dictionary<Stats, float>();
        baseBuffs = new Dictionary<Stats, float>();
        for (int i = 0; i < statList.Count; i++)
        {
            lightBuffs.Add(statList[i], 0);
            darkBuffs.Add(statList[i], 0);
            baseBuffs.Add(statList[i], baseIncrease[i]);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetNewStats(1, 1, true);
        }
    }

    //Note: Does stats buff the same of different stats depending on the dimension
    public void SetNewStats(int nrOfStatsToIncrease, int difficulty, bool noDoubledipping )
    {
        int iterations = 0;
        while(iterations < 2)
        {
            Dictionary<Stats, float> buffPool = new Dictionary<Stats, float>(); // = baseBuffs; //Linked
            foreach (var item in baseBuffs)
            {
                buffPool.Add(item.Key, item.Value);
            }
            
            for (int i = 0; i < nrOfStatsToIncrease; i++)
            {
                int index = UnityEngine.Random.Range(0, buffPool.Count);
                stat = (Stats)(index); //values.GetValue(index);
                if(iterations == 0)
                {
                    lightBuffs[stat] = buffPool[stat] * difficulty;
                }
                else
                {
                    darkBuffs[stat] = buffPool[stat] * difficulty;
                }
                
                if (noDoubledipping)
                {
                    buffPool.Remove(stat);
                }
            }
            iterations++;
        }
        
    }

    public void ApplyNewStats(bool trueDimention)
    {
        Dictionary<Stats, float> activeStats;
        if (trueDimention)
        {
            activeStats = lightBuffs;
        }
        else
        {
            activeStats = darkBuffs;
        }

        foreach (var item in activeStats)
        {
            if(item.Value > 0)
            {
                switch (item.Key)
                {
                    case Stats.Health:
                        AddMaxHealth((int)(item.Value));
                        break;
                    case Stats.DamageReduction:
                        AddDamageReduction(item.Value);
                        break;
                    case Stats.MovementSpeed:
                        AddMovementSpeed(item.Value);
                        break;
                    case Stats.Damage:
                        AddDamageIncrease(item.Value);
                        break;
                    case Stats.AttackSpeed:
                        AddAttackSpeed(item.Value);
                        break;
                    case Stats.AoE:
                        AddAreaOfEffect(item.Value);
                        break;
                }
            }
            
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TechTree trueTechTree;
    [SerializeField] TechTree mirrorTechTree;

    Dictionary<string, float> trueVariables;
    Dictionary<string, float> mirrorVariables;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        UpgradeManager[] array = FindObjectsOfType<UpgradeManager>();

        if (array.Length > 1)
            Destroy(gameObject);
    }

    void Start()
    {
        trueVariables = new Dictionary<string, float>();
        mirrorVariables = new Dictionary<string, float>();
    }

    public void GetActiveUpgrades()
    {
        trueVariables.Clear();
        mirrorVariables.Clear();
        Debug.Log(trueTechTree.GetActiveNodes().Count);

        foreach (TechTreeNode node in trueTechTree.GetActiveNodes())
        {
            Debug.Log("Active nodes");
            List<string> variables = node.GetVariables();
            List<float> values = node.GetValues();
            if (node.SpecialOne())
            {

            }
            else if (node.SpecialTwo())
            {

            }
            else if (node.SpecialThree())
            {

            }
            else if (node.SpecialFour())
            {

            }
            else if (node.IsMirror())
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    if (!mirrorVariables.ContainsKey(variables[i]))
                        mirrorVariables.Add(variables[i], values[i]);
                    else
                        mirrorVariables[variables[i]] += values[i];
                }
            }
            else
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    if (!trueVariables.ContainsKey(variables[i]))
                        trueVariables.Add(variables[i], values[i]);
                    else
                        trueVariables[variables[i]] += values[i];
                }
            }
        }
    }

    public void AddTree(TechTree tree, Dimension dimension)
    {
        if (dimension == Dimension.True)
            trueTechTree = tree;
        else
            mirrorTechTree = tree;
    }

    public Dictionary<string, float> GetTrueNodes()
    {
        Debug.Log(trueVariables.Count);
        return trueVariables;
    }

    public Dictionary<string, float> GetMirrorNodes()
    {
        Debug.Log(mirrorVariables.Count);
        return mirrorVariables;
    }
}

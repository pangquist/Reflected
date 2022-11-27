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
        UpgradeManager[] array = FindObjectsOfType<UpgradeManager>();

        if (array.Length > 1)
            Destroy(gameObject);
    }

    void Start()
    {
        trueVariables = new Dictionary<string, float>();
        mirrorVariables = new Dictionary<string, float>();
        DontDestroyOnLoad(this);
    }

    public void GetActiveUpgrades()
    {
        trueVariables.Clear();
        mirrorVariables.Clear();

        foreach (TechTreeNode node in trueTechTree.GetActiveNodes())
        {
            List<string> variables = node.GetVariables();
            List<float> values = node.GetValues();

            if (node.IsMirror())
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
        Destroy(gameObject);
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
        return trueVariables;
    }

    public Dictionary<string, float> GetMirrorNodes()
    {
        return mirrorVariables;
    }
}

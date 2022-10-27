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
            if (!trueVariables.ContainsKey(node.GetVariable()))
                trueVariables.Add(node.GetVariable(), node.GetValue());
            else
                trueVariables[node.GetVariable()] += node.GetValue();
        }

        foreach (TechTreeNode node in mirrorTechTree.GetActiveNodes())
        {
            if (!mirrorVariables.ContainsKey(node.GetVariable()))
                mirrorVariables.Add(node.GetVariable(), node.GetValue());
            else
                mirrorVariables[node.GetVariable()] += node.GetValue();
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

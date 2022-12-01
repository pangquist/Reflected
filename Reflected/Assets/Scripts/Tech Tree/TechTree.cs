using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    [SerializeField] List<TechTreeNode> activeNodes = new List<TechTreeNode>();
    List<TechTreeNode> allNodes = new List<TechTreeNode>();
    [SerializeField] bool isTrueTree;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (isTrueTree)
            GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>().AddTree(this, Dimension.True);
        else
            GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>().AddTree(this, Dimension.Mirror);

        foreach(TechTreeNode node in allNodes)
        {
            if (node.IsActive())
            {
                activeNodes.Add(node);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceNode(TechTreeNode newNode)
    {
        activeNodes.Add(newNode);
        newNode.SetIsActive(true);
    }

    public List<TechTreeNode> GetActiveNodes()
    {
        return activeNodes;
    }
}

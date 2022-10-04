using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    [SerializeField] List<TechTreeNode> activeNodes = new List<TechTreeNode>();
    List<TechTreeNode> allNodes = new List<TechTreeNode>();

    // Start is called before the first frame update
    void Start()
    {
        
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

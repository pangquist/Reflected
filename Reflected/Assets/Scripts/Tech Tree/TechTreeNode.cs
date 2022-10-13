using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeNode : MonoBehaviour
{

    [Header("Node Description")]
    [TextArea(10, 20)]
    [SerializeField] string description;

    [Header("References")]
    [SerializeField] Color deactivatedColor;
    [SerializeField] Color canBeActivatedColor;
    [SerializeField] Color activatedColor;
    [SerializeField] TechTree techTree;
    [SerializeField] List<TechTreeNode> nextNode = new List<TechTreeNode>();
    Image currentImage;

    float resourceAmount; //Will be placed in an inventoryManager
    int gemAmount;

    [SerializeField] ItemData resource;
    [SerializeField] int resourceCost;
    [SerializeField] bool hasGemCost;
    [SerializeField] ItemData gemResource;
    [SerializeField] int gemCost;
    [SerializeField] bool isPlaceable;
    bool isActive;

    [Header("Stat Changes")]
    [SerializeField] string modifiedValue;
    [SerializeField] float value;

    [SerializeField] TextMeshProUGUI nodeEffectText;
    [SerializeField] TextMeshProUGUI nodeCostText;

    Inventory inventory;

    private void Start()
    {
        //DontDestroyOnLoad(this);
        currentImage = GetComponent<Image>();
        currentImage.color = deactivatedColor;

        nodeEffectText = GameObject.Find("Node Effect Text").GetComponent<TextMeshProUGUI>();
        nodeCostText = GameObject.Find("Node Cost Text").GetComponent<TextMeshProUGUI>();

        foreach (Transform child in transform)
            nextNode.Add(child.GetComponent<TechTreeNode>());

        if (description == "")
            description = name;

        if (isPlaceable)
            currentImage.color = canBeActivatedColor;

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void PlaceInMirror()
    {
        if (!CanBePlaced())
            return;

        techTree.PlaceNode(this);
        currentImage.color = activatedColor;

        foreach (TechTreeNode node in nextNode)
        {
            if (!node.IsActive())
                node.SetIsPlaceable(true);
        }

        //resourceAmount -= resourceCost;
        inventory.Remove(resource, resourceCost);

        if (hasGemCost)
            inventory.Remove(gemResource, gemCost);
    }

    public bool CanBePlaced()
    {
        if (hasGemCost)
            return (isPlaceable && inventory.HaveEnoughCurrency(resource, resourceCost) && inventory.HaveEnoughCurrency(gemResource, gemCost));
        else
            return (isPlaceable && inventory.HaveEnoughCurrency(resource, resourceCost));
    }

    public void SetIsActive(bool state)
    {
        isActive = state;
    }

    public void SetIsPlaceable(bool state)
    {
        isPlaceable = state;
        currentImage.color = isPlaceable ? canBeActivatedColor : deactivatedColor;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public string GetVariable()
    {
        return modifiedValue;
    }

    public float GetValue()
    {
        return value;
    }

    public void SetTextToEffect()
    {
        nodeEffectText.text = description;
        if (hasGemCost)
            nodeCostText.text = inventory.GetItemAmount(resource) + " / " + resourceCost + "\n" + inventory.GetItemAmount(gemResource) + " / " + gemCost;
        else
            nodeCostText.text = inventory.GetItemAmount(resource) + " / " + resourceCost;
    }

    public void SetTextToNull()
    {
        nodeEffectText.text = "";
        nodeCostText.text = "";
    }
}

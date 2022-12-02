using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeNode : MonoBehaviour, ISavable
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

    [SerializeField] ItemData resource;
    [SerializeField] int resourceCost;
    [SerializeField] bool hasGemCost;
    [SerializeField] ItemData gemResource;
    [SerializeField] int gemCost;
    [SerializeField] bool isMirror;
    [SerializeField] bool isPlaceable;
    [SerializeField] public bool isActive;

    [SerializeField] bool specialOne;
    [SerializeField] bool specialTwo;
    [SerializeField] bool specialThree;
    [SerializeField] bool specialFour;

    [Header("Stat Changes")]
    [SerializeField] List<string> modifiedValue;
    [SerializeField] List<float> value;

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

    void Update()
    {
        if (isActive)
        {
            currentImage.color = activatedColor;
        }
        else if (isPlaceable)
        {
            currentImage.color = canBeActivatedColor;
        }
    }

    public void PlaceInMirror()
    {
        if (!CanBePlaced())
            return;

        techTree.PlaceNode(this);

        foreach (TechTreeNode node in nextNode)
        {
            if (!node.isActive)
                node.SetIsPlaceable(true);
        }

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

    public bool IsMirror()
    {
        return isMirror;
    }

    public bool SpecialOne()
    {
        return specialOne;
    }

    public bool SpecialTwo()
    {
        return specialTwo;
    }

    public bool SpecialThree()
    {
        return specialThree;
    }

    public bool SpecialFour()
    {
        return specialFour;
    }

    public List<string> GetVariables()
    {
        return modifiedValue;
    }

    public List<float> GetValues()
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

    public object SaveState()
    {
        return new SaveData()
        {
            placeable = isPlaceable,
            active = isActive
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        SetIsPlaceable(saveData.placeable);
        SetIsActive(saveData.active);
    }

    [Serializable]
    private struct SaveData
    {
        public bool placeable;
        public bool active;
    }
}

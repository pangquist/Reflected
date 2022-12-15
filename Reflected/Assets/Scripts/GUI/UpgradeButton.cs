using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Chest chest;
    [SerializeField] int index;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] Color common;
    [SerializeField] Color rare;
    [SerializeField] Color epic;
    [SerializeField] Color legendary;
    private UpgradeUi upgradeUi;
    private UiManager uiManager;
    void Start()
    {
        upgradeUi = FindObjectOfType<UpgradeUi>();
        uiManager = FindObjectOfType<UiManager>();
    }
    public int GetIndex()
    {
        return index;
    }

    public void GetUpgrade()
    {
        chest = upgradeUi.GetClosestChest();
        if (chest.GetComponent<ChestControllerPay>())
            uiManager.ShowPayChestText(false, chest.GetComponent<ChestControllerPay>().amountToPay);
        chest.OpenChest(index);
        Debug.Log("trying to spawn item");
        upgradeUi.DeactiveWindow();
    }
    public void SetButton(Chest chest, int itemIndex)
    {
        index = itemIndex;
        titleText.text = chest.GetRarity().rarity;
        descText.text = chest.GetPowerupDescriptions()[itemIndex];

        if (chest.GetRarity().rarity == "Common")
            titleText.color = common;
        else if (chest.GetRarity().rarity == "Rare")
            titleText.color = rare;
        else if (chest.GetRarity().rarity == "Epic")
            titleText.color = epic;
        else if (chest.GetRarity().rarity == "Legendary")
            titleText.color = legendary;
    }
}

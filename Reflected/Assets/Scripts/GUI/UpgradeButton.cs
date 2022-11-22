using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEditor.Rendering.LookDev;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Chest chest;
    [SerializeField] GameObject button;
    [SerializeField] int index;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;

    [SerializeField] Color common;
    [SerializeField] Color rare;
    [SerializeField] Color epic;
    [SerializeField] Color legendary;
    private UpgradeUi upgradeUi;
    private List<GameObject> buttonList;
    private List<GameObject> upgradeList;
    void Start()
    {
        chest = FindObjectOfType<Chest>();
        upgradeList = chest.GetUpgradeItems();
        upgradeUi = FindObjectOfType<UpgradeUi>();

        buttonList = upgradeUi.GetButtonList();
    }
    public int GetIndex()
    {
        return index;
    }

    public void GetUpgrade()
    {
        chest.OpenChest(index);
        Debug.Log("trying to spawn item");
        upgradeUi.DeactiveWindow();
    }
    public void SetButton(GameObject powerUp, int buttonIndex)
    {
        index = buttonIndex;
        titleText.text = powerUp.GetComponent<InteractablePowerUp>().GetRarity().rarity;

        if (powerUp.GetComponent<InteractablePowerUp>().GetRarity().rarity == "Common")
            titleText.color = common;
        else if (powerUp.GetComponent<InteractablePowerUp>().GetRarity().rarity == "Rare")
            titleText.color = rare;
        else if (powerUp.GetComponent<InteractablePowerUp>().GetRarity().rarity == "Epic")
            titleText.color = epic;
        else if (powerUp.GetComponent<InteractablePowerUp>().GetRarity().rarity == "Legendary")
            titleText.color = legendary;
        descText.text = powerUp.GetComponent<InteractablePowerUp>().GetDescription();
    }
}

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
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;
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
        chest.SpawnItem(index);
        chest.OpenChest();
        Debug.Log("trying to spawn item");
    }
    public void SetButton(GameObject powerUp, int buttonIndex)
    {
        index = buttonIndex;
        titleText.text = powerUp.GetComponent<IBuyable>().GetValue().ToString();
        //titleText.text = powerUp.GetComponent<IBuyable>().
        descText.text = powerUp.GetComponent<IBuyable>().GetDescription();

    }
}

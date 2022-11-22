using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;


public class UpgradeUi : MonoBehaviour
{
    [SerializeField] Chest chest;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject buttonObject;
    [SerializeField] List<GameObject> buttonList;

    private GameObject chestObject;
    private int buttonIndex;
    [SerializeField] List<GameObject> upgradeList;


    public void SetPanelActive()
    {
        if (chest.isOpen) return;
        upgradePanel.SetActive(true);
    }

    public void DeactiveWindow()
    {
        upgradePanel.SetActive(false);
        for (int i = 0; i < buttonList.Count; i++)
        {
            Destroy(buttonList[i]);
        }
        buttonList.Clear();
    }

    public void CreateButtons()
    {
        chest = FindObjectOfType<Chest>();
        upgradeList = chest.GetUpgradeItems();

        if (chest.isOpen) return;

        Debug.Log("upgradeList Count " + upgradeList.Count);
        Debug.Log("buttonlist Count " + buttonList.Count);
        if (buttonList.Count >= upgradeList.Count)
            return;
        for (int i = 0; i < upgradeList.Count; i++)
        {
            Debug.Log("Created 1 Button");
            buttonIndex = buttonList.Count;
            buttonList.Add(Instantiate(buttonObject, upgradePanel.transform));
            Debug.Log(upgradeList[i]);
            buttonList[i].GetComponent<UpgradeButton>().SetButton(upgradeList[i], buttonIndex);
            Debug.Log("it has index " + buttonIndex);
        }
    }
    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }
}

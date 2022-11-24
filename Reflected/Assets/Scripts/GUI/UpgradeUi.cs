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
    [SerializeField] List<GameObject> upgradeList;

    public void SetPanelActive()
    {
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<Chest>();
        if (chest.isOpen) 
            return;
        upgradePanel.SetActive(true);
        SetButtons();
    }

    public void DeactiveWindow()
    {
        upgradePanel.SetActive(false);
    }

    private void SetButtons()
    {
        upgradeList = chest.GetUpgradeItems();
        if (chest.isOpen) 
            return;
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<UpgradeButton>().SetButton(upgradeList[i], i);
        }
    }
    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }
}

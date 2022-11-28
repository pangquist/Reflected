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
    private GameObject[] chests;
    private Player player;
    public void SetPanelActive()
    {
        GetClosestChest();

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

    public Chest GetClosestChest()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        chests = GameObject.FindGameObjectsWithTag("Chest");
        for (int i = 0; i < chests.Length; i++)
        {
            if (i == 0)
            {
                if (chests[i].GetComponent<ChestControllerPay>())
                    chest = chests[i].gameObject.GetComponent<ChestControllerPay>();
                else
                    chest = chests[i].gameObject.GetComponent<Chest>();
            }
                
            else if (Vector3.Distance(chests[i].gameObject.transform.position, player.transform.position)
                < Vector3.Distance(chest.transform.position, player.transform.position))
            {
                if(chests[i].GetComponent<ChestControllerPay>())
                    chest = chests[i].gameObject.GetComponent<ChestControllerPay>();
                else
                    chest = chests[i].gameObject.GetComponent<Chest>();
            }

        }
        return chest;
    }
}

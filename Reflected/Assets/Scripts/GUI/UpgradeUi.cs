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
    // Start is called before the first frame update
    void Start()
    {
        chest = FindObjectOfType<Chest>();

    }
    

    public void DeactiveShopWindow()
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
        chestObject = GameObject.FindGameObjectWithTag("Shop").GetComponent<GameObject>();
        //upgradeList = chest.GetChestItems();

        if (buttonList.Count >= upgradeList.Count)
            return;
        for (int i = 0; i < upgradeList.Count; i++)
        {
            Debug.Log("Created 1 Button");
            buttonIndex = buttonList.Count;
            buttonList.Add(Instantiate(buttonObject, upgradePanel.transform));
            buttonList[i].GetComponent<ShopButton>().SetButton(upgradeList[i], buttonIndex);
            Debug.Log("it has index " + buttonIndex);

        }
    }

    // Update is called once per frame
    
}

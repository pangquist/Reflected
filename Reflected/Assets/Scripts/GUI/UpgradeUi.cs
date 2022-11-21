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
        chestObject = GameObject.FindGameObjectWithTag("Chest").GetComponent<GameObject>();
        //upgradeList = chest.GetChestItems();
        upgradeList = chest.GetUpgradeItems();

        Debug.Log("upgradeList Count " + upgradeList.Count);
        Debug.Log("buttonlist Count " + buttonList.Count);
        if (buttonList.Count >= upgradeList.Count)
            return;
        for (int i = 0; i < upgradeList.Count; i++)
        {
            Debug.Log("Created 1 Button");
            buttonIndex = buttonList.Count;
            buttonList.Add(Instantiate(buttonObject, upgradePanel.transform));
            buttonList[i].GetComponent<UpgradeButton>().SetButton(upgradeList[i], buttonIndex);
            Debug.Log("it has index " + buttonIndex);

        }
    }
    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }

    // Update is called once per frame

}

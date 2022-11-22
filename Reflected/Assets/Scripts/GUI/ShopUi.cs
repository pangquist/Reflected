using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class ShopUi : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject buttonObject;
    [SerializeField] List<GameObject> buttonList;
    private int buttonIndex;
    private GameObject shopObject;
    [SerializeField] List<GameObject> shopList;


    public void DeactiveWindow()
    {
        shopPanel.SetActive(false);
        for (int i = 0; i < buttonList.Count; i++)
        {
            Destroy(buttonList[i]);
        }
        buttonList.Clear();
    }

    public void CreateButtons()
    {

        for (int i = 0; i < buttonList.Count; i++)
        {
            Destroy(buttonList[i]);
        }
        buttonList.Clear();
        shop = FindObjectOfType<Shop>();
        //shopObject = GameObject.FindGameObjectWithTag("Shop").GetComponent<GameObject>();
        shopList = shop.GetShopItems();
        Debug.Log("shoplist Count " + shopList.Count);
        Debug.Log("buttonlist Count "+ buttonList.Count);
        if (buttonList.Count >= shopList.Count)
            return;
        for (int i = 0; i < shopList.Count; i++)
        {
            Debug.Log("Created 1 Button");
            buttonIndex = buttonList.Count;
            buttonList.Add(Instantiate(buttonObject, shopPanel.transform));
            buttonList[i].GetComponent<ShopButton>().SetButton(shopList[i], buttonIndex);
            Debug.Log("it has index " + buttonIndex);

        }
    }

    public void SetPanelActive()
    {
        shopPanel.SetActive(true);
    }

    public int GetButtonIndex()
    {
        return buttonIndex;
    }

    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }

    public void RemoveButtonFromList(GameObject button)
    {
        buttonList.Remove(button);
    }


    


    

}

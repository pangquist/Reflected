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
    [SerializeField] List<GameObject> buttonList;
    [SerializeField] List<GameObject> shopList;

    public void SetPanelActive()
    {
        shop = FindObjectOfType<Shop>();
        shopPanel.SetActive(true);
        for (int i = 0; i < buttonList.Count; i++)
            buttonList[i].SetActive(true);
        SetButtons();
    }

    public void DeactiveWindow()
    {
        shopPanel.SetActive(false);
    }

    private void SetButtons()
    {
        shopList = shop.GetShopItems();

        for (int i = 0; i < buttonList.Count; i++)
        {
            if (i < shopList.Count)
                buttonList[i].GetComponent<ShopButton>().SetButton(shopList[i], i);
            else
                buttonList[i].SetActive(false);
        }
    }

    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }









}

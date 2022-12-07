using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using System.Data;

public class ShopUi : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] GameObject shopPanel;
    [SerializeField] List<GameObject> buttonList;
    [SerializeField] List<GameObject> shopList;
    [SerializeField] TextMeshProUGUI coinText;
    private GameObject[] shops;
    private Player player;
    private Inventory inventory;

    public void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Update()
    {
        coinText.text = inventory.inventory[0].stackSize.ToString();
    }
    public void SetPanelActive()
    {
        shop = GetClosestShop();
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

    public Shop GetClosestShop()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        shops = GameObject.FindGameObjectsWithTag("Shop");
        for (int i = 0; i < shops.Length; i++)
        {
            if (i == 0)
                shop = shops[i].gameObject.GetComponent<Shop>();
            else if (Vector3.Distance(shops[i].gameObject.transform.position, player.transform.position)
                < Vector3.Distance(shop.transform.position, player.transform.position))
            {
                shop = shops[i].gameObject.GetComponent<Shop>();
            }

        }
        return shop;
    }

}

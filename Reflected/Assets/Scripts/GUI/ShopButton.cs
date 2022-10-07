using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Shop shop;
    [SerializeField] GameObject button;
    [SerializeField] int index;
    ShopUi shopUi;

    public List<GameObject> shopList;

    void Start()
    {
        shop = FindObjectOfType<Shop>();
        shopList = shop.GetShopItems();
        shopUi = FindObjectOfType<ShopUi>();
        index = shopUi.GetButtonIndex();
        List<GameObject> buttonList; shopUi.GetButtonList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBuyOrder()
    {
        Debug.Log("index" + index);

        shop.BuyItem(index);
        //buttonList[index].SetActive(false);

    }
}

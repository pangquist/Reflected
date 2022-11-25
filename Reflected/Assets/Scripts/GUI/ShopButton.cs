using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class ShopButton : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Shop shop;
    [SerializeField] GameObject button;
    [SerializeField] int index;
    [SerializeField] Image costImage;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] TextMeshProUGUI costText;

    private ShopUi shopUi;
    private List<GameObject> buttonList;

    void Start()
    {
        shopUi = FindObjectOfType<ShopUi>();
        buttonList = shopUi.GetButtonList();
    }
    public int GetIndex()
    {
        return index;
    }



    public void CreateBuyOrder()
    {
        shop = shopUi.GetClosestShop();
        if (shop.BuyItem(index))
        {
            buttonList[index].SetActive(false); 
        }

    }
    public void SetButton(GameObject powerUp, int buttonIndex)
    {
        index = buttonIndex;
        costText.text = powerUp.GetComponent<IBuyable>().GetValue().ToString();
        itemText.text = powerUp.GetComponent<IBuyable>().GetDescription();

    }
}

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

    private List<GameObject> shopList;

    void Start()
    {
        shop = FindObjectOfType<Shop>();
        shopList = shop.GetShopItems();
        shopUi = FindObjectOfType<ShopUi>();

        buttonList = shopUi.GetButtonList();
    }
    public int GetIndex()
    {
        return index;
    }



    public void CreateBuyOrder()
    {
        if (shop.BuyItem(index))
        {


            if (gameObject != null)
            {
                shopUi.RemoveButtonFromList(gameObject);
                Destroy(gameObject);

            }

            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].GetComponent<ShopButton>().index > index)
                {
                    buttonList[i].GetComponent<ShopButton>().index--;
                }
            }
        }

    }
    public void SetButton(GameObject powerUp, int buttonIndex)
    {
        index = buttonIndex;
        costText.text = powerUp.GetComponent<IBuyable>().GetValue().ToString();
        itemText.text = powerUp.GetComponent<IBuyable>().GetDescription();

    }
}

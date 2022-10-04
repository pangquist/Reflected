using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class ShopUi : MonoBehaviour
{
    private  Shop shop;
    [SerializeField] GameObject shopPanel;
    private GameObject shopObject;
    [SerializeField] Image CostImage;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemCost;


    public List<GameObject> shopList;

    void Start()
    {
        shopObject = GameObject.FindGameObjectWithTag("Shop").GetComponent<GameObject>();
        shop = FindObjectOfType<Shop>();
        shopList = shop.GetShopItems();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;


        for (int i = 0; i < shopList.Count; i++)
        {
            
            itemName.text = shopList[i].GetComponent<InteractablePowerUp>().powerUpEffect.description;
            itemCost.text = shopList[i].GetComponent<InteractablePowerUp>().powerUpEffect.value.ToString();

        }
    }
}

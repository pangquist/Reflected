using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUi : MonoBehaviour
{
    [SerializeField] Shop shop;

    [SerializeField] List<GameObject> shopItems;
    


    List<GameObject> shopList;

    void Start()
    {
        shopList = shop.GetShopItems();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

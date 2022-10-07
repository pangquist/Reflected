using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class ShopUi : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject buttonObject;
    private GameObject shopObject;
    [SerializeField] Image CostImage;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemCost;
    [SerializeField] List<GameObject> buttonList;
    private int buttonIndex;


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
        Cursor.lockState = CursorLockMode.None;


        //for (int i = 0; i < shopList.Count; i++)
        //{

        //    itemName.text = shopList[i].GetComponent<InteractablePowerUp>().powerUpEffect.description;
        //    itemCost.text = shopList[i].GetComponent<InteractablePowerUp>().powerUpEffect.value.ToString();

        //}
    }

    public void CreateButtons()
    {

        Debug.Log(shopList.Count);
        for (int i = 0; i < shopList.Count; i++)
        {
            Debug.Log("Created 1 Button");
            buttonIndex = buttonList.Count;
            buttonList.Add(Instantiate(buttonObject, shopPanel.transform));
            SetButtons(buttonList[i], shopList[i]);

        }
    }

    private void SetButtons(GameObject button, GameObject powerUp)
    {
        button.GetComponent<TextMeshProUGUI>().text = "hello";
        //button.GetComponent<TextMeshProUGUI>().text = powerUp.GetComponent<InteractablePowerUp>().powerUpEffect.description;
        //button.GetComponent<TextMeshProUGUI>().text = "hello";
    }

    public int GetButtonIndex()
    {
        return buttonIndex;
    }

    public List<GameObject> GetButtonList()
    {
        return buttonList;
    }


    


    

}

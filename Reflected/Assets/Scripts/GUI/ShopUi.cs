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
    [SerializeField] List<GameObject> buttonList;
    private int buttonIndex;
    private GameObject shopObject;
    public List<GameObject> shopList;

    void Start()
    {
        
        shop = FindObjectOfType<Shop>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(shopPanel.activeSelf== true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        

    }

    public void DeactiveShopWindow()
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
        shopObject = GameObject.FindGameObjectWithTag("Shop").GetComponent<GameObject>();
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

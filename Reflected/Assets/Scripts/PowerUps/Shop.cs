using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] WeightedRandomList<LootPool> lootTablePowerUps;
    [SerializeField] WeightedRandomList<LootPool> lootTableCollectables;
    [SerializeField] List<GameObject> shopItems;
    [SerializeField] Transform itemHolder;
    [SerializeField] ItemData payment;
    GameObject spawnedObject;

    //public Animator animator;
    [SerializeField] int numberOfCollectableItems = 2;
    [SerializeField] int numberOfPowerUps = 2;
    int totalNumberOfItems;
    Inventory inventory;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        
        PopulateShop();
    }

    public bool BuyItem(int index) //Have to send in an index here for the shop to give you that item
    {
        //The index you give should be put in on line 37 and 38
        inventory = FindObjectOfType<Inventory>();
        //Inventory inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        Debug.Log("Trying to buy");
        if (inventory)
        {
            Debug.Log(shopItems[0]);
            if (shopItems.Count > 0 && inventory.HaveEnoughCurrency(payment, shopItems[index].GetComponent<IBuyable>().GetValue())) 
            {
                Debug.Log("Close");
                if (spawnedObject == null)
                {
                    Debug.Log("Bought");
                    inventory.Remove(payment, shopItems[index].GetComponent<IBuyable>().GetValue());
                    SpawnItem(index);
                    return true;
                }                    
                else
                {
                    Debug.Log("Spawn not working");
                    return false;
                }
                    
            }
            else
                return false;

        }
        else
            return false;
    }

    void SpawnItem(int index)
    {
        spawnedObject = Instantiate(shopItems[index], itemHolder.position, itemHolder.rotation);
        shopItems.RemoveAt(index);
        Debug.Log("Item should spawn...");
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }

    void PopulateShop()
    {
        for (int i = 0; i < numberOfPowerUps; i++)
        {
            shopItems.Add(lootTablePowerUps.GetRandom().GetItem());
            shopItems[i].GetComponent<InteractablePowerUp>().SetProperties();
            Debug.Log("Powerup " + i + " " + shopItems[i].GetComponent<InteractablePowerUp>().GetValue());
        }

        for (int i = 0; i < numberOfCollectableItems; i++)
        {
            shopItems.Add(lootTableCollectables.GetRandom().GetItem());
        }
        Debug.Log(shopItems.Count);
        Debug.Log(shopItems[0]);

        totalNumberOfItems = numberOfPowerUps + numberOfCollectableItems;
    }

    public List<GameObject> GetShopItems()
    {
        return shopItems;
    } 
}

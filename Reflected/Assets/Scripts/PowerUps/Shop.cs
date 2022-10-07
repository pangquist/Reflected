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

    public void BuyItem(int index) //Have to send in an index here for the shop to give you that item
    {
        //The index you give should be put in on line 37 and 38
        inventory = FindObjectOfType<Inventory>();
        Debug.Log("Trying to buy");
        if (inventory)
        {
            Debug.Log(shopItems[0]);
            if (shopItems.Count > 0 && inventory.HaveEnoughCurrency(payment, shopItems[index].GetComponent<InteractablePowerUp>().powerUpEffect.value)) 
            {
                Debug.Log("Close");
                if (spawnedObject == null)
                {
                    Debug.Log("Bought");
                    inventory.Remove(payment, shopItems[index].GetComponent<InteractablePowerUp>().powerUpEffect.value);
                    SpawnItem(index);
                }                    
                else
                    Debug.Log("Spawn not working");
            }
            
        }
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

            Debug.Log("Populatad");
            shopItems.Add(lootTablePowerUps.GetRandom().GetItem());
            
        }
        Debug.Log(shopItems.Count);
        Debug.Log(shopItems[0]);
        //for (int i = 0; i < numberOfCollectableItems; i++)
        //{
        //    shopItems.Add(lootTableCollectables.GetRandom().GetItem());
        //}

        totalNumberOfItems = numberOfPowerUps; // + numberOfCollectableItems;
    }

    public List<GameObject> GetShopItems()
    {
        return shopItems;
    } 
}

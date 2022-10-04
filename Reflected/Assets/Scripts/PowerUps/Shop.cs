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


    private void Start()
    {
        //animator = GetComponent<Animator>();
        PopulateShop();
    }

    public void BuyItem(GameObject gameObject) //Have to send in an index here for the shop to give you that item
    {
        //The index you give should be put in on line 37 and 38
        Inventory inventory = gameObject.GetComponent<Inventory>();
        if (inventory)
        {
            if (shopItems.Count > 0 && inventory.HaveEnoughCurrency(payment, shopItems[totalNumberOfItems - 1].GetComponent<InteractablePowerUp>().powerUpEffect.value)) 
            {
                                               
                if (spawnedObject == null)
                {
                    inventory.Remove(payment, shopItems[totalNumberOfItems - 1].GetComponent<InteractablePowerUp>().powerUpEffect.value);
                    SpawnItem(--totalNumberOfItems);
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
            shopItems.Add(lootTablePowerUps.GetRandom().GetItem());
        }

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

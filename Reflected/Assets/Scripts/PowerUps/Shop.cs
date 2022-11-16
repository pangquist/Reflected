using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] WeightedRandomList<GameObject> lootTablePowerUps;
    [SerializeField] WeightedRandomList<GameObject> lootTableCollectables;
    [SerializeField] WeightedRandomList<Rarity> rarityTiers;
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
        //lootTablePowerUps = FindObjectOfType<LootPoolManager>().GetPowerupPool();
        //lootTableCollectables = FindObjectOfType<LootPoolManager>().GetCollectablePool();
        //foreach (var pair in lootTableCollectables.list)
        //{
        //    if (pair.item.GetComponent<IBuyable>() == null)
        //    {
        //        pair.weight = 0;
        //    }
        //}

        //rarityTiers = FindObjectOfType<LootPoolManager>().GetRarityList();
        PopulateShop();
        //Destroy(gameObject, 200);
    }

    public bool BuyItem(int index) //Have to send in an index here for the shop to give you that item
    {
        //The index you give should be put in on line 37 and 38
        inventory = FindObjectOfType<Inventory>();
        //Inventory inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        Debug.Log("Trying to buy");
        if (inventory)
        {
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
        if (spawnedObject.GetComponent<InteractablePowerUp>())
        {
            spawnedObject.GetComponent<InteractablePowerUp>().SetProperties(shopItems[index].GetComponent<InteractablePowerUp>().GetRarity());
        }
        
        shopItems.RemoveAt(index);
        Debug.Log("Item should spawn...");
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }

    void PopulateShop()
    {
        for (int i = 0; i < numberOfPowerUps; i++)
        {
            shopItems.Add(lootTablePowerUps.GetRandom());
            shopItems[i].GetComponent<InteractablePowerUp>().SetProperties(rarityTiers.GetRandom());
            //Debug.Log("Powerup " + i + ": value:" + shopItems[i].GetComponent<InteractablePowerUp>().GetValue() + ". amount: " + shopItems[i].GetComponent<InteractablePowerUp>().amount);
        }

        for (int i = 0; i < numberOfCollectableItems; i++)
        {
            shopItems.Add(lootTableCollectables.GetRandom());
        }
        totalNumberOfItems = numberOfPowerUps + numberOfCollectableItems;
    }

    public List<GameObject> GetShopItems()
    {
        return shopItems;
    } 
}

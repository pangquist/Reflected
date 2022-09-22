using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] WeightedRandomList<LootPool> lootTablePowerUps;
    //[SerializeField] WeightedRandomList<LootPool> lootTableCollectables;
    [SerializeField] List<GameObject> shopItems;
    //[SerializeField] GameObject[] shopItems = new GameObject[4];
    [SerializeField] Transform itemHolder;
    [SerializeField] ItemData payment;
    GameObject spawnedObject;

    //public Animator animator;
    //[SerializeField] int amountToPay = 1;
    [SerializeField] int numberOfCollectableItems = 2;
    [SerializeField] int numberOfPowerUps = 2;
    int totalNumberOfItems;


    private void Start()
    {
        //animator = GetComponent<Animator>();
        PopulateShop();
    }

    public void BuyItem(GameObject gameObject)
    {
        Inventory inventory = gameObject.GetComponent<Inventory>();
        if (inventory)
        {
            if (totalNumberOfItems > 0 && inventory.HaveEnoughCurrency(payment, shopItems[totalNumberOfItems - 1].GetComponent<InteractablePowerUp>().powerUpEffect.value)) //
            {
                inventory.Remove(payment, shopItems[totalNumberOfItems - 1].GetComponent<InteractablePowerUp>().powerUpEffect.value);                                
                if (spawnedObject == null)
                    SpawnItem(--totalNumberOfItems);
                else
                    Debug.Log("Spawn not working");
            }
        }
    }

    void SpawnItem(int index)
    {
        //GameObject item = lootTable.GetRandom();
        spawnedObject = Instantiate(shopItems[index], itemHolder.position, itemHolder.rotation);
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
        //    shopItems.Add(lootTablePowerUps.GetRandom().GetItem());
        //}

        totalNumberOfItems = numberOfPowerUps; // + numberOfCollectableItems;
    }
}

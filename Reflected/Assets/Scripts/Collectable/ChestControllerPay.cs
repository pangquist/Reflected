using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestControllerPay : MonoBehaviour
{
    [SerializeField] WeightedRandomList<LootPool> lootTable;
    [SerializeField] Transform itemHolder;
    [SerializeField] ItemData payment;
    GameObject spawnedObject;

    public bool isOpen;
    //public Animator animator;
    [SerializeField] public int amountToPay = 1;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void OpenChest(GameObject gameObject)
    {
        Inventory inventory = gameObject.GetComponent<Inventory>();
        if (inventory)
        {
            if (!isOpen && inventory.HaveEnoughCurrency(payment, amountToPay)) //
            {
                isOpen = true;
                inventory.Remove(payment, amountToPay);
                Debug.Log("Chest is now open...");
                if (spawnedObject == null)
                    SpawnItem();
            }
        }        
    }

    void SpawnItem()
    {
        //GameObject item = lootTable.GetRandom();
        spawnedObject = Instantiate(lootTable.GetRandom().GetItem(), itemHolder.position, itemHolder.rotation);
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }
}

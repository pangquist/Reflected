using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestControllerPay : Chest
{
    [SerializeField] ItemData payment;
    [SerializeField] public int amountToPay = 1;

    public override void OpenChest(int index)
    {
        Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        //amountToPay = itemToSpawn.GetComponent<InteractablePowerUp>().powerUpEffect.value;
        if (inventory)
        {
            if (!isOpen && inventory.HaveEnoughCurrency(payment, amountToPay)) 
            {
                isOpen = true;
                inventory.Remove(payment, amountToPay);
                Debug.Log("Chest is now open...");
                if (spawnedObject == null)
                    SpawnItem(index);

                Destroy(gameObject, 20);
            }
        } 
    }
}

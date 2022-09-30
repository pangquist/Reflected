using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestControllerPay : Chest
{
    [SerializeField] ItemData payment;
    [SerializeField] public int amountToPay = 1;

    public override void OpenChest(GameObject gameObject)
    {
        Inventory inventory = gameObject.GetComponent<Inventory>();
        if (inventory)
        {
            if (!isOpen && inventory.HaveEnoughCurrency(payment, amountToPay)) 
            {
                isOpen = true;
                inventory.Remove(payment, amountToPay);
                Debug.Log("Chest is now open...");
                if (spawnedObject == null)
                    SetItems();
            }
        }
        
    }
}

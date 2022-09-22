using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour//, ISavable
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    //[SerializeField]
    public Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable() //Subscribing to events
    {
        MirrorShard.OnShardCollected += Add;
        Coin.OnCoinCollected += Add;
        Diamond.OnDiamondCollected += Add;
    }

    private void OnDisable()
    {
        MirrorShard.OnShardCollected -= Add;
        Coin.OnCoinCollected -= Add;
        Diamond.OnDiamondCollected -= Add;
    }

    public void Add(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddMoreToStack(itemData.amount);
            Debug.Log($"{item.itemData.displayName} total stack is now {item.stackSize}");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($" Added {itemData.displayName} to the inventory for the first time");
        }
    }

    public void Remove(ItemData itemData, int amount)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (item.stackSize >= amount)
            {
                item.RemoveMoreFromStack(amount);
                if (item.stackSize == 0)
                {
                    inventory.Remove(item);
                    itemDictionary.Remove(itemData);
                }
            }
            
        }
    }

    public bool HaveEnoughCurrency(ItemData itemData, int amount)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            return item.stackSize >= amount;
        }
        else
        {
            Debug.Log("Need more " + itemData.displayName + " to use this");
        }

        return false;
    }

    //public object SaveState()
    //{
    //    return new SaveData()
    //    {
    //        foreach(var v in i)
    //        inventory = this.inventory,
    //        itemDictionary = this.itemDictionary
    //    };
    //}

    //public void LoadState(object state)
    //{
    //    var saveData = (SaveData)state;
    //    inventory = saveData.inventory;
    //    itemDictionary = saveData.itemDictionary;
    //}

    //[Serializable]
    //private struct SaveData
    //{
    //    public string displayName;
    //    public int amount;
    //    //public List<InventoryItem> inventory;
    //    //public Dictionary<ItemData, InventoryItem> itemDictionary;
    //}
}

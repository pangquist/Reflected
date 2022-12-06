using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] ItemDatabaseData dataBase;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    //[SerializeField]
    public Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Inventory[] array = FindObjectsOfType<Inventory>();
        
        if (array.Length > 1)
            Destroy(gameObject);

        if(inventory.Count <= 0)
        {
            for (int i = 0; i < dataBase.items.Length; i++)
            {
                InventoryItem newItem = new InventoryItem(dataBase.items[i]);
                inventory.Add(newItem);
                itemDictionary.Add(newItem.itemData, newItem);
            }
        }
        
    }

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
            //Debug.Log($"{item.itemData.displayName} total stack is now {item.stackSize}");
        }
    }

    public void Remove(ItemData itemData, int amount)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (item.stackSize >= amount)
            {
                item.RemoveMoreFromStack(amount);
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

    public int GetItemAmount(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            return item.stackSize;
        }
        else
            return 0;
    }

    public void ResetTemporaryCollectables()
    {
        inventory[0].RemoveMoreFromStack(inventory[0].stackSize);
    }

    public object SaveState()
    {
        return new SaveData()
        {
            //coinAmount = inventory[0].stackSize,
            diamondAmount = inventory[1].stackSize,
            mirrorShardAmount = inventory[2].stackSize,
            trueMirrorShardAmount = inventory[3].stackSize
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        //inventory[0].SetStackSize(saveData.coinAmount);
        inventory[1].SetStackSize(saveData.diamondAmount);
        inventory[2].SetStackSize(saveData.mirrorShardAmount);
        inventory[3].SetStackSize(saveData.trueMirrorShardAmount);
    }

    [Serializable]
    private struct SaveData
    {
        //public int coinAmount;
        public int diamondAmount;
        public int mirrorShardAmount;
        public int trueMirrorShardAmount;
    }


}

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

    public void Awake()
    {
        for (int i = 0; i < dataBase.items.Length; i++)
        {
            InventoryItem newItem = new InventoryItem(dataBase.items[i]);
            inventory.Add(newItem);
            itemDictionary.Add(newItem.itemData, newItem);
        }
    }

    private void OnEnable() //Subscribing to events
    {
//#if UNITY_EDITOR
//        dataBase = (ItemDatabaseData)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Inventory/Database.asset", typeof(ItemDatabaseData));
//#else
//        dataBase = Resources.Load<ItemDatabaseData>("Database");
//#endif
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

    public object SaveState()
    {
        return new SaveData()
        {
            coinAmount = inventory[0].stackSize,
            diamondAmount = inventory[1].stackSize,
            mirrorShardAmount = inventory[2].stackSize
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        inventory[0].AddMoreToStack(saveData.coinAmount);
        inventory[1].AddMoreToStack(saveData.diamondAmount);
        inventory[2].AddMoreToStack(saveData.mirrorShardAmount);
    }

    [Serializable]
    private struct SaveData
    {
        public int coinAmount;
        public int diamondAmount;
        public int mirrorShardAmount;        
    }


}

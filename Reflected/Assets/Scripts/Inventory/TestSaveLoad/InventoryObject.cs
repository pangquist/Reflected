using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    private ItemDatabaseObject dataBase;
    public List<InventorySlot> container = new List<InventorySlot>();

    private void OnEnable() //Subscribing to events
    {
#if UNITY_EDITOR
        dataBase = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        dataBase = Resources.Load<ItemDatabaseObject>("Database");
#endif
        //MirrorShard.OnShardCollected += AddItem;
        //Coin.OnCoinCollected += AddItem;
        //Diamond.OnDiamondCollected += AddItem;
    }

    //private void OnDisable()
    //{
    //    MirrorShard.OnShardCollected -= AddItem;
    //    Coin.OnCoinCollected -= AddItem;
    //    Diamond.OnDiamondCollected -= AddItem;
    //}

    public void AddItem(ItemObject item) //Change to itemData and we should be good here
    {
        for (int i = 0; i < container.Count; i++)
        {
            if(container[i].item == item)
            {
                container[i].AddAmount(item.amount);
                return;
            }
        }
        container.Add(new InventorySlot(dataBase.GetId[item] ,item, item.amount));
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < container.Count; i++)
        {
            container[i].item = dataBase.GetItem[container[i].id];
        }
    }

    public void OnBeforeSerialize()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class InventorySlot
{
    public int id;
    public ItemObject item;
    public int amount;
    public InventorySlot(int id, ItemObject item, int amount)
    {
        id = this.id;
        item = this.item;
        amount = this.amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}

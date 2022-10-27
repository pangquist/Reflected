using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Data/Database")]
public class ItemDatabaseData : ScriptableObject
{
    public ItemData[] items;
    //public Dictionary<ItemData, int> GetId = new Dictionary<ItemData, int>();
    //public Dictionary<int, ItemData> GetItem = new Dictionary<int, ItemData>();

    //public void OnAfterDeserialize()
    //{
    //    GetId = new Dictionary<ItemData, int>();
    //    GetItem = new Dictionary<int, ItemData>();
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        GetId.Add(items[i], i);
    //        GetItem.Add(i, items[i]);
    //    }
    //}
}

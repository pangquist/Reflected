using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //Alows it to show up in the inspector
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}

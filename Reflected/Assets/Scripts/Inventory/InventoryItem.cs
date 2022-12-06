using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //Alows it to show up in the inspector
public class InventoryItem
{
    [SerializeField] public ItemData itemData;
    [SerializeField] public int stackSize;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        //AddToStack();
        //AddMoreToStack(item.amount);
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void AddMoreToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveMoreFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void SetStackSize(int amount)
    {
        stackSize = amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    [SerializeField] Inventory playerInventory;
    int coinCount;

    private void Awake()
    {
        coinText.text = $"{playerInventory.inventory[0].itemData.displayName}: {playerInventory.inventory[0].stackSize}";
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += IncrementCoinCount;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= IncrementCoinCount;
    }

    public void IncrementCoinCount(ItemData itemData)
    {
        coinCount++;
        if (playerInventory.itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            coinText.text = $"{item.itemData.displayName}: {item.stackSize}";
        }        
        //coinText.text = $"Coin: {coinCount}";
    }

    public void DecrementCoinCount(ItemData itemData)
    {
        coinCount--;
        coinText.text = $"{itemData.displayName}: {coinCount}";
    }
}

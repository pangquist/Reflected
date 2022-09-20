using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    int coinCount;

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
        coinText.text = $"{itemData.displayName}: {coinCount}";
        //coinText.text = $"Coin: {coinCount}";
    }

    public void DecrementCoinCount(ItemData itemData)
    {
        coinCount--;
        coinText.text = $"{itemData.displayName}: {coinCount}";
    }
}

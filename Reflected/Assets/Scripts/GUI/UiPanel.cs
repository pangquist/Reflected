using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UiPanel : MonoBehaviour
{
    [Header("PlayerStats")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI attackspeedText;

    [Header("Currencys")]
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI trueShard;
    [SerializeField] TextMeshProUGUI mirrorShard;
    [SerializeField] TextMeshProUGUI gems;

    private PlayerStatSystem statSystem;
    private Player player;
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        statSystem = player.GetComponent<PlayerStatSystem>();
        healthText.text = "Health: " + (player.GetMaxHealth() + player.playerStats().GetMaxHealthIncrease()).ToString();
        damageText.text = "Damage: " + (player.GetDamage() * player.playerStats().GetDamageIncrease()).ToString();
        speedText.text = "Movement Speed: " + (player.GetMovementSpeed() * player.playerStats().GetMovementSpeed()).ToString();
        attackspeedText.text = "Attack Speed: " + (player.GetAttackSpeed() * player.playerStats().GetAttackSpeed()).ToString();
        coin.text = inventory.inventory[0].stackSize.ToString();
        gems.text = inventory.inventory[1].stackSize.ToString();
        mirrorShard.text = inventory.inventory[2].stackSize.ToString();
        trueShard.text = inventory.inventory[2].stackSize.ToString();
    }
}

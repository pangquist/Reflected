using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UiPanel : MonoBehaviour
{
    [Header("PlayerStats")]
    [SerializeField] TextMeshProUGUI damageReductionText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI attackspeedText;
    [SerializeField] TextMeshProUGUI cooldownDecrease;

    [Header("Currencys")]
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI trueShard;
    [SerializeField] TextMeshProUGUI mirrorShard;
    [SerializeField] TextMeshProUGUI gems;

    [Header("Room Info")]
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI clearedRoomsText;
    [SerializeField] TextMeshProUGUI averageTimeText;


    private PlayerStatSystem statSystem;
    private Player player;
    private AiDirector aiDirector;
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        aiDirector = FindObjectOfType<AiDirector>();
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        statSystem = player.GetComponent<PlayerStatSystem>();
        damageReductionText.text = "Damage Reduction: " + player.playerStats().GetDamageReduction().ToString();
        damageText.text = "Damage: " + (player.GetDamage() * player.playerStats().GetDamageIncrease()).ToString();
        speedText.text = "Movement Speed: " + (player.GetMovementSpeed() * player.playerStats().GetMovementSpeed()).ToString();
        attackspeedText.text = "Attack Speed: " + (player.GetAttackSpeed() * player.playerStats().GetAttackSpeed()).ToString();
        cooldownDecrease.text = "Cooldown Decrease: " + player.playerStats().GetCooldownDecrease().ToString();

        if (inventory == null)
            return;

        coin.text = inventory.inventory[0].stackSize.ToString();
        gems.text = inventory.inventory[1].stackSize.ToString();
        mirrorShard.text = inventory.inventory[3].stackSize.ToString();
        trueShard.text = inventory.inventory[2].stackSize.ToString();

        killCountText.text = "Kill Count: " + aiDirector.GetKillCount().ToString();
        clearedRoomsText.text = "Cleared Rooms: " + aiDirector.GetClearedRooms().ToString();
        averageTimeText.text = "Average Room Clear Time: " + aiDirector.GetAverageTime().ToString("0.00") + " s";
    }
}

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
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI attackSpeedText;
    [SerializeField] TextMeshProUGUI damageReductionText;
    [SerializeField] TextMeshProUGUI cooldownText;

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

    [SerializeField] TextMeshProUGUI timerText;

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
        damageText.text = "Damage: " + (player.GetDamage() * player.playerStats().GetDamageIncrease()).ToString();
        speedText.text = "Movement Speed: " + (player.GetMovementSpeed() * player.playerStats().GetMovementSpeed()).ToString();
        attackSpeedText.text = "Attack Speed: " + (player.GetAttackSpeed() * player.playerStats().GetAttackSpeed()).ToString();
        damageReductionText.text = "Damage Reduction: " + (1 - player.playerStats().GetDamageReduction()).ToString();
        cooldownText.text = "Cooldown: " + (1 - player.playerStats().GetCooldownDecrease()).ToString();

        if (inventory == null)
            return;

        coin.text = inventory.inventory[0].stackSize.ToString();
        gems.text = inventory.inventory[1].stackSize.ToString();
        mirrorShard.text = inventory.inventory[3].stackSize.ToString();
        trueShard.text = inventory.inventory[2].stackSize.ToString();

        killCountText.text = "Kill Count: " + aiDirector.GetKillCount().ToString();
        clearedRoomsText.text = "Cleared Rooms: " + aiDirector.GetClearedRooms().ToString();
        averageTimeText.text = "Average Room Clear Time: " + Math.Round(aiDirector.GetAverageTime()).ToString() + " s";

        timerText.text = "Run Timer: " + Math.Round(aiDirector.GetRunTime()).ToString() + " s";
    }
}

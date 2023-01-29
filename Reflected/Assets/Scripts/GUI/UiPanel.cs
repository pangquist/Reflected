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

    [SerializeField] TextMeshProUGUI timerText;

    private float minute;
    private float second;
    private float boolTimer;
    bool doOnce;
    private PlayerStatSystem statSystem;
    private Player player;
    private AiDirector aiDirector;
    private GameManager gameManager;
    void Awake()
    {
        doOnce = true;
        gameManager = FindObjectOfType<GameManager>();  
        inventory = FindObjectOfType<Inventory>();
        aiDirector = FindObjectOfType<AiDirector>();
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        statSystem = player.GetComponent<PlayerStatSystem>();
        damageReductionText.text = "Damage Reduction: +" + ((1 - player.playerStats().GetDamageReduction()) * 100).ToString() + " %";
        damageText.text = "Damage: " + (player.GetDamage() * player.playerStats().GetDamageIncrease()).ToString();
        speedText.text = "Movement Speed: +" + ((1 - player.playerStats().GetMovementSpeed()) * 100).ToString() + " %";
        attackspeedText.text = "Attack Speed: +" + ((1 - player.playerStats().GetAttackSpeed()) * 100).ToString() + " %"; //player.GetAttackSpeed() * 
        cooldownDecrease.text = "Cooldown Decrease: -" + ((1 - player.playerStats().GetCooldownDecrease()) * 100).ToString() + " %";

        if (inventory == null)
            return;

        coin.text = inventory.inventory[0].stackSize.ToString();
        gems.text = inventory.inventory[1].stackSize.ToString();
        mirrorShard.text = inventory.inventory[3].stackSize.ToString();
        trueShard.text = inventory.inventory[2].stackSize.ToString();

        killCountText.text = "Kill Count: " + aiDirector.GetKillCount().ToString();
        clearedRoomsText.text = "Cleared Rooms: " + aiDirector.GetClearedRooms().ToString();
        averageTimeText.text = "Average Room Clear Time: " + aiDirector.GetAverageTime().ToString("0.00") + " s";

        if (!doOnce && Mathf.Round(gameManager.GetRunTimer()) % 60 == 0)
        {
            minute++;
            doOnce = true;
        }

        if (doOnce)
        {
            boolTimer += Time.deltaTime;

            if (boolTimer >= 10)
            {
                doOnce = false;
                boolTimer = 0;
            }
        }

        second = gameManager.GetRunTimer() % 60;
        timerText.text = "Run Timer: " + minute.ToString() + "m " + second.ToString("0.0") + "s";
    }
}

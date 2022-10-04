using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    
    private ItemData itemData;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.GetCurrentHealth().ToString();
        damageText.text = "Damage: " + player.GetDamage().ToString();
        speedText.text = "Speed: " + player.GetMovementSpeed().ToString();
        attackspeedText.text = "Attack Speed: " + player.GetAttackSpeed().ToString();
        coin.text = inventory.inventory[0].stackSize.ToString();
        gems.text = inventory.inventory[1].stackSize.ToString();
        mirrorShard.text = inventory.inventory[2].stackSize.ToString();
    }
}

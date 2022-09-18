using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI attackspeedText;
    
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.currentHealth.ToString();
        damageText.text = "Damage: " + player.currentHealth.ToString();
        speedText.text = "Speed: " + player.GetMovementSpeed().ToString();
        attackspeedText.text = "Attack Speed: " + player.currentHealth.ToString();
    }
}

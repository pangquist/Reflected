using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Image fillImage;
    [SerializeField] Image bgImage;
    [SerializeField] Color healthBarColor;
    [SerializeField] Color healthBarBGColor;
    [SerializeField] TextMeshProUGUI healthText;
    Player player;
    



    // Start is called before the first frame update
    void Awake()
    {
        fillImage.color = healthBarColor;
        bgImage.color = healthBarBGColor;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthBar.maxValue = player.GetMaxHealth();
    }
        // Update is called once per frame
        void Update()
    {

        healthText.text = player.GetCurrentHealth().ToString() + "/" + player.GetMaxHealth().ToString();
        //healthBar.fillRect.
        healthBar.value = player.GetCurrentHealth();
        //healthBar.image.color = healthBarColor;
        //healthBar.fillRect.GetComponent<Image>().color = healthBarColor;
        //healthBar.handleRect.GetComponent<Image>().color = healthBarColor;
    }
    public void HealthbarUpdate()
    {
        healthBar.value = player.GetCurrentHealth();
        fillImage.color = healthBarColor;
    }
}

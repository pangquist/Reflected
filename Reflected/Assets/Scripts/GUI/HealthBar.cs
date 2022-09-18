using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Image fillImage;
    [SerializeField] Image bgImage;
    [SerializeField] Color healthBarColor;
    [SerializeField] Color healthBarBGColor;
    Player player;
    



    // Start is called before the first frame update
    void Awake()
    {
        fillImage.color = healthBarColor;
        bgImage.color = healthBarBGColor;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthBar.maxValue = player.maxHealth;
    }
        // Update is called once per frame
        void Update()
    {
        
        
        //healthBar.fillRect.
        healthBar.value = player.currentHealth;
        //healthBar.image.color = healthBarColor;
        //healthBar.fillRect.GetComponent<Image>().color = healthBarColor;
        //healthBar.handleRect.GetComponent<Image>().color = healthBarColor;
    }
    public void HealthbarUpdate()
    {
        healthBar.value = player.currentHealth;
        fillImage.color = healthBarColor;
    }
}

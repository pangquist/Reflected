using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour
{
    [Header("Ability 1")]
    [SerializeField] Image ability1Image;
    [SerializeField] TextMeshProUGUI ability1text;
    [Header("Ability 2")]
    [SerializeField] Image ability2Image;
    [SerializeField] TextMeshProUGUI ability2text;

    private Weapon weapon;
    private Player player;
    private ThirdPersonMovement thirdPersonMovement;
    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        thirdPersonMovement = FindObjectOfType<ThirdPersonMovement>();
        weapon = player.GetCurrentWeapon();
        ability1Image.fillAmount = 0;
        ability2Image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2(); 
        
    }

    public void Ability1Use()
    {
        ability1Image.fillAmount = 1;
        ability1text.gameObject.SetActive(true);
    }

    public void Ability2Use()
    {
        ability2Image.fillAmount = 1;
        ability2text.gameObject.SetActive(true);
    }

    private void Ability1()
    {
        if (weapon.IsOnCooldown())
        {
            ability1text.text = Mathf.RoundToInt(weapon.GetCurrentCooldown()).ToString();
            ability1Image.fillAmount -= 1 / weapon.GetCooldown() * Time.deltaTime;
        }
        else if (!weapon.IsOnCooldown())
        {
            ability1text.gameObject.SetActive(false);
            ability1Image.fillAmount = 0;
        }
        if (ability1Image.fillAmount <= 0)
        {
            ability1Image.fillAmount = 0;
        }


    }

    private void Ability2()
    {
        if (thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2text.text = Mathf.RoundToInt(thirdPersonMovement.GetDash().GetCooldown()).ToString();
            ability2Image.fillAmount -= 1 / thirdPersonMovement.GetDash().GetCooldown() * Time.deltaTime;
        }
        else if (!thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2text.gameObject.SetActive(false);
            ability2Image.fillAmount = 0;
        }
        if (ability2Image.fillAmount <= 0)
        {
            ability2Image.fillAmount = 0;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour
{
    [Header("Ability 1")]
    [SerializeField] Image ability1Icon;
    [SerializeField] Image ability1FillImage;
    [SerializeField] TextMeshProUGUI ability1text;
    [Header("Ability 2")]
    [SerializeField] Image ability2Icon;
    [SerializeField] Image ability2FillImage;
    [SerializeField] TextMeshProUGUI ability2text;

    private Weapon weapon;
    private Player player;
    [SerializeField] private ThirdPersonMovement thirdPersonMovement;
    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        thirdPersonMovement = FindObjectOfType<ThirdPersonMovement>();
        weapon = player.GetCurrentWeapon();
        ability1FillImage.fillAmount = 0;
        ability2FillImage.fillAmount = 0;

        ability2Icon.sprite = thirdPersonMovement.GetDash().GetIcon();
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2(); 
        
    }

    public void Ability1Use()
    {
        ability1FillImage.fillAmount = 1;
        ability1text.gameObject.SetActive(true);
    }

    public void Ability2Use()
    {
        ability2FillImage.fillAmount = 1;
        ability2text.gameObject.SetActive(true);
    }

    private void Ability1()
    {
        if (weapon.IsOnCooldown())
        {
            ability1text.text = Mathf.RoundToInt(weapon.GetCurrentCooldown()).ToString();
            ability1FillImage.fillAmount -= 1 / weapon.GetCooldown() * Time.deltaTime;
        }
        else if (!weapon.IsOnCooldown())
        {
            ability1text.gameObject.SetActive(false);
            ability1FillImage.fillAmount = 0;
        }
        if (ability1FillImage.fillAmount <= 0)
        {
            ability1FillImage.fillAmount = 0;
        }


    }

    private void Ability2()
    {
        if (thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2text.text = Mathf.RoundToInt(thirdPersonMovement.GetDash().GetRemainingCooldown()).ToString();
            ability2FillImage.fillAmount = thirdPersonMovement.GetDash().GetCooldownPercentage();
        }
        else if (!thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2text.gameObject.SetActive(false);
            ability2FillImage.fillAmount = 0;
        }
        if (ability2FillImage.fillAmount <= 0)
        {
            ability2FillImage.fillAmount = 0;
        }

    }
}

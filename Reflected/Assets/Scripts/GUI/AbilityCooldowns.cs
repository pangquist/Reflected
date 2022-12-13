using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour
{
    [Header("Ability 1")]
    [SerializeField] Image ability1Icon;
    [SerializeField] Image ability1Overlay;
    [SerializeField] TextMeshProUGUI ability1Text;

    [Header("Ability 2")]
    [SerializeField] Image ability2Icon;
    [SerializeField] Image ability2Overlay;
    [SerializeField] TextMeshProUGUI ability2Text;

    [Header("Ability 3")]
    [SerializeField] Image ability3Icon;
    [SerializeField] TextMeshProUGUI ability3Text;

    [Header("Other")]
    [SerializeField] private ThirdPersonMovement thirdPersonMovement;

    private Weapon weapon;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        thirdPersonMovement = FindObjectOfType<ThirdPersonMovement>();
        weapon = player.GetCurrentWeapon();
        ability1Overlay.fillAmount = 0;
        ability2Overlay.fillAmount = 0;

        ability1Icon.sprite = player.GetSpecialAbility().GetIcon();
        ability2Icon.sprite = thirdPersonMovement.GetDash().GetIcon();

        //ability3Icon.sprite = player.GetSwapAbility().GetIcon();
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2(); 
        
    }

    public void Ability1Use()
    {
        ability1Overlay.fillAmount = 1;
        ability1Text.gameObject.SetActive(true);
    }

    public void Ability2Use()
    {
        ability2Overlay.fillAmount = 1;
        ability2Text.gameObject.SetActive(true);
    }

    private void Ability1()
    {
        Ability ability = player.GetSpecialAbility();

        if (ability.IsOnCooldown())
        {
            ability1Text.text = Mathf.CeilToInt(ability.GetRemainingCooldown()).ToString();
            ability1Overlay.fillAmount = ability.GetCooldownPercentage();
        }
        else
        {
            ability1Text.gameObject.SetActive(false);
            ability1Overlay.fillAmount = 0;
        }

        if (ability1Overlay.fillAmount <= 0)
        {
            ability1Overlay.fillAmount = 0;
        }
    }

    private void Ability2()
    {
        if (thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2Text.text = Mathf.CeilToInt(thirdPersonMovement.GetDash().GetRemainingCooldown()).ToString();
            ability2Overlay.fillAmount = thirdPersonMovement.GetDash().GetCooldownPercentage();
        }
        else if (!thirdPersonMovement.GetDash().IsOnCooldown())
        {
            ability2Text.gameObject.SetActive(false);
            ability2Overlay.fillAmount = 0;
        }
        if (ability2Overlay.fillAmount <= 0)
        {
            ability2Overlay.fillAmount = 0;
        }

    }
}

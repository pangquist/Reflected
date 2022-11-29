using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUi : MonoBehaviour
{
    [Header("Freeze Effect")]
    [SerializeField] GameObject freezeUi;
    [SerializeField] Image freezeIcon;
    [SerializeField] Image freezeFillImage;
    [SerializeField] TextMeshProUGUI freezeCountdowntext;
    [SerializeField] TextMeshProUGUI numberOfFreezetext;
    [SerializeField] TextMeshProUGUI freezeAmountText;

    [Header("Burn Effect")]
    [SerializeField] GameObject burnUi;
    [SerializeField] Image burnIcon;
    [SerializeField] Image burnFillImage;
    [SerializeField] TextMeshProUGUI burnCountdowntext;
    [SerializeField] TextMeshProUGUI numberOfBurntext;
    [SerializeField] TextMeshProUGUI burnAmountText;

    [Header("Life Regen")]
    [SerializeField] GameObject lifeRegenUi;
    [SerializeField] Image lifeRegenIcon;
    [SerializeField] Image lifeRegenFillImage;
    [SerializeField] TextMeshProUGUI lifeRegenCountdowntext;
    [SerializeField] TextMeshProUGUI numberOfLifeRegentext;
    [SerializeField] TextMeshProUGUI lifeRegenAmountText;

    Character character;
    int numberOfFreeze;
    int numberOfBurn;
    int numberOfLifeRegen;
    List<Effect> statusEffects;

    // Start is called before the first frame update
    void Start()
    {
        character = FindObjectOfType<Character>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckEffects();
    }

    void CheckEffects()
    {
        numberOfFreeze = 0;
        numberOfBurn = 0;
        numberOfLifeRegen = 0;
        statusEffects = character.GetStatusEffectList();

        foreach (Effect effect in statusEffects)
        {
            if (effect.effect.MovementPenalty > 0)
            {
                numberOfFreeze++;
                FreezeEffect(effect);
            }
            if (effect.effect.DOTAmount > 0)
            {
                numberOfBurn++;
                BurnEffect(effect);
            }
            if (effect.effect.DOTAmount < 0)
            {
                numberOfLifeRegen++;
                LifeRegen(effect);
            }
        }

        if (numberOfFreeze > 0)
            freezeUi.SetActive(true);
        else
            freezeUi.SetActive(false);

        if (numberOfBurn > 0)
            burnUi.SetActive(true);
        else
            burnUi.SetActive(false);

        if (numberOfLifeRegen > 0)
            lifeRegenUi.SetActive(true);
        else
            lifeRegenUi.SetActive(false);
    }

    void FreezeEffect(Effect effect)
    {
        freezeFillImage.fillAmount = effect.currentEffectTime / effect.effect.LifeTime;
        freezeCountdowntext.text = Mathf.CeilToInt(effect.effect.LifeTime - effect.currentEffectTime).ToString();
        numberOfFreezetext.text = "x" + numberOfFreeze.ToString();
        //freezeAmountText.text = (character.MovementPenalty() * 100).ToString() + "%";
    }

    void BurnEffect(Effect effect)
    {
        burnFillImage.fillAmount = effect.currentEffectTime / effect.effect.LifeTime;
        burnCountdowntext.text = Mathf.CeilToInt(effect.effect.LifeTime - effect.currentEffectTime).ToString();
        numberOfBurntext.text = "x" + numberOfBurn.ToString();
        //burnAmountText.text = (effect.effect.DOTAmount * numberOfBurn * -1).ToString();
    }

    void LifeRegen(Effect effect)
    {
        lifeRegenFillImage.fillAmount = effect.currentEffectTime / effect.effect.LifeTime;
        lifeRegenCountdowntext.text = Mathf.CeilToInt(effect.effect.LifeTime - effect.currentEffectTime).ToString();
        numberOfLifeRegentext.text = "x" + numberOfLifeRegen.ToString();
        //lifeRegenAmountText.text = (effect.effect.DOTAmount * numberOfLifeRegen * -1).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaticHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Character character;
    
    [Header("Values")]
    [SerializeField] private HealthTextMode healthTextMode;
    [SerializeField] private Gradient gradient;

    private void Start()
    {
        if (healthTextMode == HealthTextMode.None)
            text.text = "";

        if (character != null)
            SetCharacter(character);
        else
            text.text = "";
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        character.HealthChanged.AddListener(UpdateHealthBar);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        Debug.Log("Updating static health bar " + character.GetHealthPercentage().ToString("0.000"));
        slider.value = character.GetHealthPercentage();
        fill.color = gradient.Evaluate(slider.value / slider.maxValue);

        if (healthTextMode == HealthTextMode.Current)
            text.text = (int)(character.GetCurrentHealth() + 0.5f) + "";

        else if (healthTextMode == HealthTextMode.Full)
            text.text = (int)(character.GetCurrentHealth() + 0.5f) + "/" + (int)(character.GetMaxHealth() + 0.5f);
    }

}

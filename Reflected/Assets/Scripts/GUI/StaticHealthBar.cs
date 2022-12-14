using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaticHealthBar : HealthBar
{
    [Header("References")]
    [SerializeField] private Character character;

    private void Start()
    {
        if (character != null)
            SetCharacter(character);
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        character.HealthChanged.AddListener(() => UpdateHealthBar(character));
        UpdateHealthBar(character);
    }

}

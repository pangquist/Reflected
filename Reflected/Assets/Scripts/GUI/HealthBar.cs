using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public abstract class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Slider delayedSlider;
    [SerializeField] protected Image fill;
    [SerializeField] protected Image delayedFill;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Animator animator;

    [Header("Values")]
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected HealthTextMode healthTextMode;

    public Slider Slider => slider;
    public Image Fill => fill;
    public TextMeshProUGUI Text => text;
    public Gradient Gradient => gradient;

    private void Awake()
    {
        text.text = "";
    }

    private void Update()
    {
        if (delayedSlider.value > slider.value)
        {
            delayedSlider.value -= (0.1f + (delayedSlider.value - slider.value) * 4) * Time.unscaledDeltaTime;

            if (delayedSlider.value < slider.value)
                delayedSlider.value = slider.value;

            delayedFill.color = fill.color;
        }
    }

    public void UpdateHealthBar(Character character)
    {
        slider.value = character.GetHealthPercentage();
        fill.color = gradient.Evaluate(slider.value / slider.maxValue);

        if (delayedSlider.value < slider.value)
        {
            delayedSlider.value = slider.value;
            delayedFill.color = fill.color;
        }

        if (healthTextMode == HealthTextMode.Current)
            text.text = (int)(character.GetCurrentHealth() + 0.5f) + "";

        else if (healthTextMode == HealthTextMode.Full)
            text.text = (int)(character.GetCurrentHealth() + 0.5f) + "/" + (int)(character.GetMaxHealth() + 0.5f);

        SingleShake();
    }

    public void SingleShake()
    {
        animator.SetTrigger("Single Shake");
    }

    public void StartShake()
    {
        animator.SetTrigger("Start Shake");
    }

    public void StopShake()
    {
        animator.SetTrigger("Stop Shake");
    }

}

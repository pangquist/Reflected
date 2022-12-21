using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PostProcessingManager : MonoBehaviour
{
    [Header("Volume and profiles")]
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile trueProfile;
    [SerializeField] private VolumeProfile mirrorProfile;

    [Header("Vignette")]
    [SerializeField] private float maxHealthPercentage;
    [SerializeField] private float maxVignetteValue;
    [SerializeField] private float onDamagedVignetteIntensityIncrease;
    [SerializeField] private float vignetteFadeSpeed;

    private Player player;
    private Vignette trueVignette;
    private Vignette mirrorVignette;
    private float vignetteIntensityTargetValue;
    private float oldPlayerHealth;

    private void Awake()
    {
        trueProfile.TryGet<Vignette>(out trueVignette);
        mirrorProfile.TryGet<Vignette>(out mirrorVignette);

        if (SceneManager.GetActiveScene().name == "Start Scene")
            return;

        player = GameObject.Find("Player").GetComponentInChildren<Player>();
        player.HealthChanged.AddListener(UpdateVignetteTarget);
        oldPlayerHealth = player.GetCurrentHealth();
    }

    public void SwapProfile()
    {
        volume.profile = trueProfile ? mirrorProfile : trueProfile;
    }

    public void UseTrueProfile()
    {
        volume.profile = trueProfile;
    }

    public void UseMirrorProfile()
    {
        volume.profile = mirrorProfile;
    }

    private void Update()
    {
        // Move vignette intensity value towards target value

        if (trueVignette.intensity.value != vignetteIntensityTargetValue)
        {
            float change = (vignetteIntensityTargetValue - trueVignette.intensity.value) * Mathf.Min(1f, Time.unscaledDeltaTime * vignetteFadeSpeed);
            trueVignette.intensity.value   += change;
            mirrorVignette.intensity.value += change;

            if (Mathf.Abs(trueVignette.intensity.value - vignetteIntensityTargetValue) < 0.01f)
            {
                trueVignette  .intensity.value = vignetteIntensityTargetValue;
                mirrorVignette.intensity.value = vignetteIntensityTargetValue;
            }
        }
    }

    private void UpdateVignetteTarget()
    {
        if (player.GetCurrentHealth() < oldPlayerHealth)
        {
            trueVignette  .intensity.value = Mathf.Min(  trueVignette.intensity.max,   trueVignette.intensity.value + onDamagedVignetteIntensityIncrease);
            mirrorVignette.intensity.value = Mathf.Min(mirrorVignette.intensity.max, mirrorVignette.intensity.value + onDamagedVignetteIntensityIncrease);
        }

        oldPlayerHealth = player.GetCurrentHealth();
        vignetteIntensityTargetValue = Mathf.Abs(1 - player.GetHealthPercentage().ValueToPercentageClamped(0f, maxHealthPercentage)).PercentageToValue(0f, maxVignetteValue);
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Start Scene")
            return;

        vignetteIntensityTargetValue = 0f;
        trueVignette.intensity.value = 0f;
        mirrorVignette.intensity.value = 0f;
    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == "Start Scene")
            return;

        vignetteIntensityTargetValue = 0f;
        trueVignette.intensity.value = 0f;
        mirrorVignette.intensity.value = 0f;
    }
}

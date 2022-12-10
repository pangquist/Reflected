using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Setting
{
    Quality,
    WindowMode,
    SoundVolume,
    MenuMusicVolume,
    InGameMusicVolume
}

public class Settings : MonoBehaviour
{
    // Options

    public enum Quality { Low, Medium, High }
    public enum WindowMode { Windowed, Fullscreen }

    [Header("Increments")]
    [SerializeField] float soundVolumeIncrement;
    [SerializeField] float menuMusicVolumeIncrement;
    [SerializeField] float inGameMusicVolumeIncrement;

    // Active options

    [Header("Active options")]
    [SerializeField] Quality quality;
    [SerializeField] WindowMode windowMode;
    [SerializeField] float soundVolume;
    [SerializeField] float menuMusicVolume;
    [SerializeField] float inGameMusicVolume;

    // Default options

    [Header("Default options")]
    [SerializeField] Quality defaultQuality;
    [SerializeField] WindowMode defaultWindowMode;
    [SerializeField] float defaultSoundVolume;
    [SerializeField] float defaultMenuMusicVolume;
    [SerializeField] float defaultInGameMusicVolume;

    [Header("References")]
    [SerializeField] AudioMixer mixer;

    // Get option

    public Quality GetQuality() => quality;
    public WindowMode GetWindowMode() => windowMode;
    public float GetSoundVolume() => soundVolume;
    public float GetMenuMusicVolume() => menuMusicVolume;
    public float GetInGameMusicVolume() => inGameMusicVolume;

    public void ApplyDefaultSettings()
    {
        quality = defaultQuality;
        windowMode = defaultWindowMode;
        soundVolume = defaultSoundVolume;
        menuMusicVolume = defaultMenuMusicVolume;
        inGameMusicVolume = defaultInGameMusicVolume;

        ApplyQuality();
        ApplyWindowMode();
        ApplyVolume("SoundEffectVolume", soundVolume);
        ApplyVolume("MenuMusicVolume", menuMusicVolume);
        ApplyVolume("InGameMusicVolume", inGameMusicVolume);
    }

    public string NextOption(Setting setting)
    {
        switch (setting)
        {
            case Setting.Quality:
                Increment(ref quality);
                ApplyQuality();
                return GetString(quality);

            case Setting.WindowMode:
                Increment(ref windowMode);
                ApplyWindowMode();
                return GetString(windowMode);

            case Setting.SoundVolume:
                Increment(ref soundVolume, soundVolumeIncrement);
                ApplyVolume("SoundEffectVolume", soundVolume);
                return GetPercentage(soundVolume);

            case Setting.MenuMusicVolume:
                Increment(ref menuMusicVolume, menuMusicVolumeIncrement);
                ApplyVolume("MenuMusicVolume", menuMusicVolume);
                return GetPercentage(menuMusicVolume);

            case Setting.InGameMusicVolume:
                Increment(ref inGameMusicVolume, inGameMusicVolumeIncrement);
                ApplyVolume("InGameMusicVolume", inGameMusicVolume);
                return GetPercentage(inGameMusicVolume);
        }

        return "Error";
    }

    private void Increment(ref float value, float increment)
    {
        if (value >= 1.0f)
            value = 0.0f;
        else
            value = Mathf.Min(1.0f, value + increment);
    }

    private void Increment<T>(ref T value) where T : struct
    {
        value = value.GetNext();
    }

    public string GetPercentage(float value)
    {
        return (value * 100.0f).ToString("0") + " %";
    }

    public string GetString<T>(T value) where T : struct
    {
        return value.ToString().SplitPascalCase();
    }

    private void ApplyQuality()
    {
        QualitySettings.SetQualityLevel((int)quality, false);
    }

    private void ApplyWindowMode()
    {
        
    }

    private void ApplyVolume(string mixerName, float value)
    {
        if (mixer != null)
            mixer.SetFloat(mixerName, Mathf.Log10(value) * 30);
    }

}

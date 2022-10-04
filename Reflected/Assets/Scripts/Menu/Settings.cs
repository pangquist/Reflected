using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Setting
{
    WindowMode,
    SoundVolume,
    MenuMusicVolume,
    InGameMusicVolume
}

public class Settings : MonoBehaviour
{
    // Options

    public enum WindowMode { Windowed, Fullscreen }

    [Header("Increments")]
    [SerializeField] float soundVolumeIncrement;
    [SerializeField] float menuMusicVolumeIncrement;
    [SerializeField] float inGameMusicVolumeIncrement;

    // Active options

    [Header("Active options")]
    [SerializeField] WindowMode windowMode;
    [SerializeField] float soundVolume;
    [SerializeField] float menuMusicVolume;
    [SerializeField] float inGameMusicVolume;

    // Default options

    [Header("Default options")]
    [SerializeField] WindowMode defaultWindowMode;
    [SerializeField] float defaultSoundVolume;
    [SerializeField] float defaultMenuMusicVolume;
    [SerializeField] float defaultInGameMusicVolume;

    [Header("References")]
    [SerializeField] AudioMixer mixer;

    // Get option

    public WindowMode GetWindowMode() => windowMode;
    public float GetSoundVolume() => soundVolume;
    public float GetMenuMusicVolume() => menuMusicVolume;
    public float GetInGameMusicVolume() => inGameMusicVolume;

    void Start()
    {

    }

    public void ApplyDefaultSettings()
    {
        windowMode = defaultWindowMode;
        soundVolume = defaultSoundVolume;
        menuMusicVolume = defaultMenuMusicVolume;
        inGameMusicVolume = defaultInGameMusicVolume;
    }

    public string NextOption(Setting setting)
    {
        switch (setting)
        {
            case Setting.WindowMode:
                return Increment(ref windowMode);

            case Setting.SoundVolume:
                    return GetVolumeValue(Increment(ref soundVolume, soundVolumeIncrement), "SoundEffectVolume");

            case Setting.MenuMusicVolume:
                    return GetVolumeValue(Increment(ref menuMusicVolume, menuMusicVolumeIncrement), "MenuMusicVolume");

            case Setting.InGameMusicVolume:
                    return GetVolumeValue(Increment(ref inGameMusicVolume, inGameMusicVolumeIncrement), "InGameMusicVolume");
        }

        return "Error";
    }

    private string Increment(ref float value, float increment)
    {
        if (value >= 1.0f)
            value = 0.0f;
        else
            value = Mathf.Min(1.0f, value + increment);

        return GetPercentage(value);
    }

    private string Increment<T>(ref T value) where T : struct
    {
        value = value.GetNext();
        return GetString(value);
    }

    public string GetPercentage(float value)
    {
        return (value * 100.0f).ToString("0") + " %";
    }

    public string GetString<T>(T value) where T : struct
    {
        return value.ToString().SplitPascalCase();
    }

    public string GetVolumeValue(string stringValue, string mixerName)
    {
        stringValue = stringValue.Split(' ')[0];
        float value = float.Parse(stringValue);

        if (value == 0)
            value = 0.01f;

        mixer.SetFloat(mixerName, Mathf.Log10(value / 100) * 30);

        return stringValue + " %";
    }

}

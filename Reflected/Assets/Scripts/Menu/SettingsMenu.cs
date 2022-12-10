using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenu : Menu
{
    [SerializeField] Settings settings;
    [SerializeField] GameObject grid;

    protected override void Start()
    {
        UpdateOptionTexts();
    }

    protected override void Update()
    {
        if (menuManager.ActiveMenu != this)
            return;

        if (playerController.Back())
            OnClick_Back();
    }

    protected override void OnEnable()
    {
        
    }

    protected override void OnDisable()
    {

    }

    public void OnClick_Back()
    {
        menuManager.SwapMenu(this, Type.Main);
    }

    public void OnClick_Default()
    {
        settings.ApplyDefaultSettings();
        UpdateOptionTexts();
    }

    public void OnClick_Setting(int setting)
    {
        string option = settings.NextOption((Setting)setting);
        EventSystem.current.currentSelectedGameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = option;
    }

    private void UpdateOptionTexts()
    {
        SetSettingButtonText("Window Mode",          settings.GetString(    settings.GetWindowMode()));
        SetSettingButtonText("Sound Volume",         settings.GetPercentage(settings.GetSoundVolume()));
        SetSettingButtonText("Menu Music Volume",    settings.GetPercentage(settings.GetMenuMusicVolume()));
        SetSettingButtonText("In-Game Music Volume", settings.GetPercentage(settings.GetInGameMusicVolume()));
    }

    private void SetSettingButtonText(string settingGameObject, string text)
    {
        grid.transform.Find(settingGameObject).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }

}

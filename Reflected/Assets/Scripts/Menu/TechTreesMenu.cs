using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreesMenu : Menu
{
    protected override void Start()
    {

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
        GameObject.Find("SaveLoadSystem").GetComponent<SaveLoadSystem>().Save();
        menuManager.SwapMenu(this, Type.Main);
    }
}

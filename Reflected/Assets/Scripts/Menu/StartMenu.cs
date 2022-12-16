using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : Menu
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (menuManager.ActiveMenu != this)
            return;

        if (playerController.Back())
            Exit.ExitApplication();

        else if (Input.anyKeyDown)
        {
            menuManager.SwapMenu(this, Type.Main);
            GameObject.Find("SaveLoadSystem").GetComponent<SaveLoadSystem>().Load();
            this.gameObject.SetActive(false);
        }
            
    }

    protected override void OnEnable()
    {

    }

    protected override void OnDisable()
    {

    }

}

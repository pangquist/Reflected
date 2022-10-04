using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenu : Menu
{
    [SerializeField] private string gameSceneName;

    protected override void Start()
    {

    }

    protected override void Update()
    {
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

    public void OnClick_Start()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    string gameSceneName = "Game Scene";

    private void Awake()
    {
        GameObject.Find("SaveLoadSystem").GetComponent<SaveLoadSystem>().Load();
    }

    protected override void Start()
    {
        CheckForSavedRun();
    }

    protected override void Update()
    {
        if (playerController.Back())
            OnClick_Exit();
    }

    protected override void OnEnable()
    {
        CheckForSavedRun();
    }

    protected override void OnDisable()
    {

    }

    private void CheckForSavedRun()
    {
        bool savedRun = false;

        transform.Find("Buttons").Find("Continue").GetComponent<Button>().SetEnabled(savedRun);
    }

    public void OnClick_Continue()
    {
        // Start saved run
    }

    public void OnClick_NewGame()
    {
        menuManager.SwapMenu(this, Type.NewGame);
    }

    public void OnClick_TechTrees()
    {
        menuManager.SwapMenu(this, Type.TechTrees);
    }

    public void OnClick_Settings()
    {
        menuManager.SwapMenu(this, Type.Settings);
    }

    public void OnClick_Credits()
    {
        menuManager.SwapMenu(this, Type.Credits);
    }

    public void OnClick_Exit()
    {
        Exit.ExitApplication();
    }

    public void OnClick_Start()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(gameSceneName);
    }

}

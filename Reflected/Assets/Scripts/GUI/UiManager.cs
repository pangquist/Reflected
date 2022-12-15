using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager: MonoBehaviour
{

    public enum MenuState
    {
        Active,
        Inactive,
    }
    public enum UiState
    {
        Active,
        Inactive,
    }

    public UiState state;
    public MenuState menuState;
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject CurrencyPanel;
    [SerializeField] GameObject roominfoPanel;
    [SerializeField] GameObject timerPanel;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject payChestText;
    [SerializeField] Image tint;
    private CinemachineFreeLook camera;

    public Image Tint => tint;

    // Start is called before the first frame update
    void Start()
    {
        shopPanel.SetActive(false);
        uiPanel.SetActive(false);
        CurrencyPanel.SetActive(false);
        roominfoPanel.SetActive(false);
        timerPanel.SetActive(false);
        interactText.SetActive(false);
        inGameMenu.SetActive(false);
        tutorialPanel.SetActive(false);
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForESC();
        CheckForTab();
        HandleMouse();
    }

    void CheckForTab()
    {
        if (inGameMenu.activeSelf)
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CurrencyPanel.SetActive(true);
            uiPanel.SetActive(true);
            roominfoPanel.SetActive(true);
            timerPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            uiPanel.SetActive(false);
            CurrencyPanel.SetActive(false);
            roominfoPanel.SetActive(false);
            timerPanel.SetActive(false);
        }
    }

    void CheckForESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGameMenu.activeSelf)
            {
                inGameMenu.SetActive(false);
            }
            else if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 0;
                shopPanel.SetActive(false);
                uiPanel.SetActive(false);
                CurrencyPanel.SetActive(false);
                roominfoPanel.SetActive(false);
                timerPanel.SetActive(false);
                interactText.SetActive(false);
                inGameMenu.SetActive(true);
                tutorialPanel.SetActive(false);
            }
        }
    }

    void HandleMouse()
    {
        if(inGameMenu.activeSelf)
            menuState = MenuState.Active;
        if (inGameMenu.activeSelf || shopPanel.activeSelf || upgradePanel.activeSelf || tutorialPanel.activeSelf)
        {
            state = UiState.Active;
        }
        else
        {
            state = UiState.Inactive;
            menuState = MenuState.Inactive;
        }
          
        if(state == UiState.Active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            camera.enabled = false;

        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            camera.enabled = true;
        }
    }

    public void Continue()
    {
        inGameMenu.SetActive(false);
    }

    public void BackToMenu()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Save();
        SceneManager.LoadScene(0);
    }

    public void TutorialMenu()
    {
        tutorialPanel.SetActive(true);
        inGameMenu.SetActive(false);
    }

    public void Quit()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Save();
        Exit.ExitApplication();
    }

    public void ShowInteractText(bool boolean)
    {
        interactText.SetActive(boolean);
    }

    public void ShowPayChestText(bool boolean, int value)
    {
        if (value == 0)
            return;
        payChestText.SetActive(boolean);
        if (value == 1)
            payChestText.GetComponent<TextMeshProUGUI>().text = "Pay " + value + " coin to open the chest";
        else
            payChestText.GetComponent<TextMeshProUGUI>().text = "Pay " + value + " coins to open the chest";
    }

    public MenuState GetMenuState()
    {
        return menuState;
    }
}

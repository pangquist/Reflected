using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager: MonoBehaviour
{

    enum UiState
    {
        Active,
        Inactive,
    }

    private UiState state;
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject CurrencyPanel;
    [SerializeField] GameObject roominfoPanel;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject payChestText;
    private CinemachineFreeLook camera;


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
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
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            uiPanel.SetActive(false);
            CurrencyPanel.SetActive(false);
            roominfoPanel.SetActive(false);
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
                shopPanel.SetActive(false);
                uiPanel.SetActive(false);
                CurrencyPanel.SetActive(false);
                roominfoPanel.SetActive(false);
                interactText.SetActive(false);
                inGameMenu.SetActive(true);
            }
        }
        

    }

    void HandleMouse()
    {
        if (inGameMenu.activeSelf || shopPanel.activeSelf || upgradePanel.activeSelf)
        {
            state = UiState.Active;
        }
        else
        {
            state = UiState.Inactive;
        }
          
        if(state == UiState.Active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            camera.enabled = false;

        }
        else
        {
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
}

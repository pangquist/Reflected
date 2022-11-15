using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject upgradePanel;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
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
        if (inGameMenu.active)
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CurrencyPanel.SetActive(true);
            uiPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            uiPanel.SetActive(false);
            CurrencyPanel.SetActive(false);
        }
    }

    void CheckForESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGameMenu.active)
            {
                inGameMenu.SetActive(false);
            }
            else if (!inGameMenu.active)
            {
                uiPanel.SetActive(false);
                CurrencyPanel.SetActive(false);
                inGameMenu.SetActive(true);
            }
        }
        

    }

    void HandleMouse()
    {
        if (inGameMenu.activeSelf == true || shopPanel.activeSelf == true || upgradePanel.activeSelf == true)
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
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Continue()
    {
        inGameMenu.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}

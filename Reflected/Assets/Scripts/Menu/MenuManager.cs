using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Menu startMenu, mainMenu, newGameMenu, techTreesMenu, settingsMenu, creditsMenu;
    [SerializeField] private PlayerController playerController;

    private Dictionary<Menu.Type, Menu> menus;

    public PlayerController PlayerController => playerController;

    private void Start()
    {
        menus = new Dictionary<Menu.Type, Menu>()
        {
            { Menu.Type.Start, startMenu },
            { Menu.Type.Main, mainMenu },
            { Menu.Type.NewGame, newGameMenu },
            { Menu.Type.TechTrees, techTreesMenu },
            { Menu.Type.Settings, settingsMenu },
            { Menu.Type.Credits, creditsMenu }
        };

        foreach (KeyValuePair<Menu.Type, Menu> menu in menus)
            menu.Value.gameObject.SetActive(false);

        startMenu.gameObject.SetActive(true);
    }

    public void SwapMenu(Menu callerMenu, Menu.Type menu)
    {
        callerMenu.gameObject.SetActive(false);
        menus[menu].gameObject.SetActive(true);
    }

}

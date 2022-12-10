using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Menu startMenu, mainMenu, newGameMenu, techTreesMenu, settingsMenu, creditsMenu;
    [SerializeField] private PlayerController playerController;

    private Dictionary<Menu.Type, Menu> menus;
    private Menu activeMenu;

    public PlayerController PlayerController => playerController;
    public Menu ActiveMenu => activeMenu;

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

        activeMenu = startMenu;

        Vector2 pos = new Vector2();

        foreach (KeyValuePair<Menu.Type, Menu> menu in menus)
        {
            pos = menu.Value.gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.x += 10000;
            menu.Value.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        //menu.Value.gameObject.SetActive(false);

        //startMenu.gameObject.SetActive(true);

        pos = startMenu.gameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x -= 10000;
        startMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void SwapMenu(Menu callerMenu, Menu.Type menu)
    {
        Vector2 pos = callerMenu.gameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x += 10000;
        callerMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;

        pos = menus[menu].gameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x -= 10000;
        menus[menu].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;

        activeMenu = null;
        StartCoroutine(Coroutine_SetActiveMenu(menu));

        //callerMenu.gameObject.SetActive(false);
        //menus[menu].gameObject.SetActive(true);
    }

    private IEnumerator Coroutine_SetActiveMenu(Menu.Type menu)
    {
        yield return null;
        activeMenu = menus[menu];
        yield return 0;
    }



}

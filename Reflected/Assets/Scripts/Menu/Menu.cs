using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public enum Type { Start, Main, NewGame, TechTrees, Settings, Credits }

    static protected MenuManager menuManager;

    protected PlayerController playerController => menuManager.PlayerController;

    protected virtual void Start()
    {
        if (menuManager == null)
            menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    protected virtual void Update()
    {
        
    }

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    protected void Disable()
    {
        gameObject.SetActive(false);
    }
}

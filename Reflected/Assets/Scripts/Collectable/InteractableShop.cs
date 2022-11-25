using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableShop : Interactable
{
    private ShopUi shopUi;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        Player.OnObjectInteraction += Interact;
        shopUi = FindObjectOfType<ShopUi>();
    }

    void Interact()
    {
        if (isInRange)
        {
            shopUi.SetPanelActive();
            uiManager.ShowInteractText(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
            isInRange = true;
            uiManager.ShowInteractText(true);
            //Debug.Log("Player now in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager.ShowInteractText(false);
            shopUi.DeactiveWindow();
            isInRange = false;
        }
    }
}


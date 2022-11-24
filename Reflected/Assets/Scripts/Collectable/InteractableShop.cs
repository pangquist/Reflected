using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableShop : Interactable
{
    private ShopUi shopUi;
    Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player.OnObjectInteraction += Interact;
        shopUi = FindObjectOfType<ShopUi>();
    }

    void Interact()
    {
        if (isInRange)
        {
            shopUi.SetPanelActive();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shopUi.DeactiveWindow();
            isInRange = false;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class InteractableChest : Interactable
{
    private UpgradeUi upgradeUi;
    private Chest chest;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        Player.OnObjectInteraction += Interact;
        upgradeUi = FindObjectOfType<UpgradeUi>();
    }  

    void Interact()
    {
        if (isInRange)
        {
            upgradeUi.SetPanelActive();
            uiManager.ShowInteractText(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        chest = upgradeUi.GetClosestChest();
        if (chest.isOpen)
            return;
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
            upgradeUi.DeactiveWindow();
            isInRange = false;
        }
    }
}

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

    protected override void Interact()
    {
        if (isInRange)
        {
            if (chest.GetComponent<ChestControllerPay>())
            {

                uiManager.ShowPayChestText(false, chest.GetComponent<ChestControllerPay>().amountToPay);
            }
            upgradeUi.SetPanelActive();
            
            uiManager.ShowInteractText(false);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        chest = upgradeUi.GetClosestChest();
        if (chest.isOpen)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
            isInRange = true;
            if(chest.GetComponent<ChestControllerPay>())
            {
                uiManager.ShowPayChestText(true, chest.GetComponent<ChestControllerPay>().amountToPay);
            }
            uiManager.ShowInteractText(true);
            //Debug.Log("Player now in range");
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (chest.GetComponent<ChestControllerPay>())
            {
                uiManager.ShowPayChestText(false, chest.GetComponent<ChestControllerPay>().amountToPay);
            }
            uiManager.ShowInteractText(false);
            upgradeUi.DeactiveWindow();
            isInRange = false;
        }
    }
}

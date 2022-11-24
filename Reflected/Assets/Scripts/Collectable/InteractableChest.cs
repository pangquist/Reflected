using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableChest : Interactable
{
    private UpgradeUi upgradeUi;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player.OnObjectInteraction += Interact;
        upgradeUi = FindObjectOfType<UpgradeUi>();
    }  

    void Interact()
    {
        if (isInRange)
        {
            upgradeUi.SetPanelActive();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            upgradeUi.DeactiveWindow();
            isInRange = false;
        }
    }
}

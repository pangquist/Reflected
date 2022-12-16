using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDummy : Interactable
{
    private TutorialUI tutorialUI;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        //Player.OnObjectInteraction += Interact;
        tutorialUI = FindObjectOfType<TutorialUI>();
    }

    private void OnEnable()
    {
        Player.OnObjectInteraction += Interact;
    }

    private void OnDisable()
    {
        Player.OnObjectInteraction -= Interact;
    }

    protected override void Interact()
    {
        if (isInRange)
        {
            tutorialUI.SetPanelActive();
            uiManager.ShowInteractText(false);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
            isInRange = true;
            uiManager.ShowInteractText(true);
            //Debug.Log("Player now in range");
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager.ShowInteractText(false);
            tutorialUI.SetPanelInactive();
            isInRange = false;
        }
    }
}

